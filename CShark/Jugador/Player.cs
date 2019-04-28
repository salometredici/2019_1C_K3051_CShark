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

        public TgcFpsCamera CamaraInterna { get; private set; }
        private TgcD3dInput Input;


        public Player(TGCVector3 posicion, int vidaInicial, int oxigenoInicial, TgcD3dInput input) {
            Inventario = new Inventario();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            HUD = new HUD(Vida, Oxigeno);
            Input = input;
            CamaraInterna = new TgcFpsCamera(posicion, input);
            Arma = new Cañon();
            onPause = false;
        }
        
        public void Update(GameModel game) {
            if (!onPause)
            {
                Posicion = CamaraInterna.Position;
                Oxigeno -= 7f * game.ElapsedTime;
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
