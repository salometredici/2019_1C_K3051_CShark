using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using CShark.UI.HUD;
using CShark.Utils;
using CShark.Model;
using TGC.Core.Input;
using TGC.Core.Geometry;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using Microsoft.DirectX.DirectInput;
using CShark.Variables;
using CShark.Managers;
using CShark.Items;

namespace CShark.Jugador
{
    public class Player
    {
        public float Vida;
        public float Oxigeno;
        private Inventario Inventario;
        private HUD HUD;
        private Arma Arma;
        public TGCVector3 Posicion { get; private set; }
        public bool EstaVivo => Vida > 0 && Oxigeno > 0;
        public bool onPause;

        private Variable<float> VelocidadMovimiento;
        
        public TgcFpsCamera CamaraInterna { get; private set; }
        public TgcD3dInput Input;
        public TGCVector3 MoveVector;

        public Player(TGCVector3 posicion, int vidaInicial, int oxigenoInicial, TgcD3dInput input) {
            Inventario = new Inventario();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            HUD = new HUD(Vida, Oxigeno);
            Input = input;
            CamaraInterna = new TgcFpsCamera(input, this);
            Arma = new Crossbow();
            onPause = false;
            VelocidadMovimiento = Configuracion.Instancia.VelocidadMovimiento;
        }
        
        public void Update(GameModel game) {
            if (!onPause)
            {
                Posicion = CamaraInterna.Position;

                MoveVector = TGCVector3.Empty;

                if (Input.keyDown(Key.W))
                    MoveVector += new TGCVector3(0, 0, -1) * VelocidadMovimiento.Valor;
                if (Input.keyDown(Key.S))
                    MoveVector += new TGCVector3(0, 0, 1) * VelocidadMovimiento.Valor;
                if (Input.keyDown(Key.D))
                    MoveVector += new TGCVector3(-1, 0, 0) * VelocidadMovimiento.Valor;
                if (Input.keyDown(Key.A))
                    MoveVector += new TGCVector3(1, 0, 0) * VelocidadMovimiento.Valor;
                                
                ActualizarOxigeno(game);
                if (EstaVivo)
                {
                    Arma.Update(game);
                    HUD.Update(Vida, Oxigeno);
                }
                else
                {
                    _murio = true;
                    BloquearCamara(CamaraInterna);
                }
            }          
        }

        public void Recoger(Recolectable item) {
            Inventario.Agregar(item);
        }

        private void ActualizarOxigeno(GameModel game)
        {
            Oxigeno = Posicion.Y >= 2800f && Oxigeno < HUD.BarraOxigeno.ValorMaximo ?
                Oxigeno += 14f * game.ElapsedTime :
                Oxigeno -= 7f * game.ElapsedTime;
        }

        private bool _murio = false;
        private void BloquearCamara(TgcFpsCamera camara) {
            if (!_murio)
            {
                camara.Lock();
                //_murio = true;
            }
        }

        public void Lock() {
            BloquearCamara(CamaraInterna);
        }
        
        public void Render() {
            if (EstaVivo)
            {
                Arma.Render();
                HUD.Render();
            }
        }

    }
}
