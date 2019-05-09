using TGC.Core.Mathematica;
using CShark.Model;
using TGC.Core.Input;
using Microsoft.DirectX.DirectInput;
using CShark.Variables;
using CShark.Managers;
using CShark.Items;
using BulletSharp;
using TGC.Core.Geometry;
using System.Drawing;
using TGC.Core.Collision;
using CShark.Terreno;
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;

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
        private TgcBoundingSphere Esfera;
        private TGCVector3 _posAnterior;

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
            Esfera = new TgcBoundingSphere(posicion, 500f);
            Esfera.setRenderColor(Color.Purple);
        }

        private void CheckCollisions() {
            Esfera.setCenter(Posicion);
            var arregloX = new TGCVector3(50, 0, 0);
            var arregloY = new TGCVector3(0, 50, 0);
            var arregloZ = new TGCVector3(0, 0, 50);
            foreach (var pared in Mapa.Instancia.ParedesBoundaries) {
                if (TgcCollisionUtils.testSphereAABB(Esfera, pared)) {
                    MoveVector = Posicion - _posAnterior;
                    MoveVector += pared.Position.X < _posAnterior.X ? arregloX : -arregloX;
                    MoveVector += pared.Position.Y < _posAnterior.Y ? arregloY : -arregloY;
                    MoveVector += pared.Position.Z < _posAnterior.Z ? arregloZ : -arregloZ;
                    //esto es una basura, hacerlo bien despues ::::)))
                }
            }
        }
        
        public void Update(GameModel game) {
            if (!onPause)
            {
                Posicion = CamaraInterna.Position;

                MoveVector = TGCVector3.Empty;

                if (Input.keyDown(Key.W)) {
                    MoveVector += new TGCVector3(0, 0, -1) * VelocidadMovimiento.Valor;
                }
                    
                if (Input.keyDown(Key.S)) {
                    MoveVector += new TGCVector3(0, 0, 1) * VelocidadMovimiento.Valor;
                }
                    
                if (Input.keyDown(Key.D)) {
                    MoveVector += new TGCVector3(-1, 0, 0) * VelocidadMovimiento.Valor;
                }
                    
                if (Input.keyDown(Key.A)) {
                    MoveVector += new TGCVector3(1, 0, 0) * VelocidadMovimiento.Valor;
                }
                    
                CheckCollisions();

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

                _posAnterior = new TGCVector3(Posicion.X, Posicion.Y, Posicion.Z);
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
                Esfera.Render();
                Arma.Render();
                HUD.Render();
            }
        }

    }
}
