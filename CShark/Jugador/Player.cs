using TGC.Core.Mathematica;
using CShark.Model;
using TGC.Core.Input;
using Microsoft.DirectX.DirectInput;
using CShark.Variables;
using CShark.Managers;
using CShark.Items;
using BulletSharp;
using CShark.Terreno;
using TGC.Core.BulletPhysics;
using CShark.Utils;
using BulletSharp.Math;
using CShark.Items.Crafteables;

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

        private RigidBody Capsula;

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
            Capsula = BulletRigidBodyFactory.Instance.CreateCapsule(300, 500, posicion, 5, false);
            Capsula.SetDamping(0.1f, 0f);
            Capsula.Restitution = 0.1f;
            Capsula.Friction = 0.3f;
            Mapa.Instancia.AgregarBody(Capsula);
        }
        
        private void MoverCapsula(float elapsedTime, TgcD3dInput input) {
            var strength = 10.30f;

            if (input.keyDown(Key.W)) {
                Capsula.ActivationState = ActivationState.ActiveTag;
                Capsula.AngularVelocity = TGCVector3.Empty.ToBulletVector3();
                var cos = FastMath.Cos(CamaraInterna.leftrightRot);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot);
                Capsula.ApplyCentralImpulse(-strength * new Vector3(sin, 0, cos));
            }

            if (input.keyDown(Key.S)) {
                Capsula.ActivationState = ActivationState.ActiveTag;
                Capsula.AngularVelocity = TGCVector3.Empty.ToBulletVector3();
                var cos = FastMath.Cos(CamaraInterna.leftrightRot);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot);
                Capsula.ApplyCentralImpulse(strength * new Vector3(sin, 0, cos));
            }

            if (input.keyDown(Key.D)) {
                Capsula.ActivationState = ActivationState.ActiveTag;
                Capsula.AngularVelocity = TGCVector3.Empty.ToBulletVector3();
                var cos = FastMath.Cos(CamaraInterna.leftrightRot + FastMath.PI / 2);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot + FastMath.PI / 2);
                Capsula.ApplyCentralImpulse(-strength * new Vector3(sin, 0, cos));
            }

            if (input.keyDown(Key.A)) {
                Capsula.ActivationState = ActivationState.ActiveTag;
                Capsula.AngularVelocity = TGCVector3.Empty.ToBulletVector3();
                var cos = FastMath.Cos(CamaraInterna.leftrightRot + FastMath.PI / 2);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot + FastMath.PI / 2);
                Capsula.ApplyCentralImpulse(strength * new Vector3(sin, 0, cos));
            }


            if (input.keyPressed(Key.Space) && !jumping) {
                if (RozandoSuperficie || !Sumergido) {
                    jumping = true;
                    Capsula.ActivationState = ActivationState.ActiveTag;
                    Capsula.ApplyCentralImpulse(new TGCVector3(0, 500 * strength, 0).ToBulletVector3());
                }
            }

            //nadar lentamente hacia arriba
            if (input.keyDown(Key.Space) && Sumergido) {
                Capsula.ActivationState = ActivationState.ActiveTag;
                Capsula.AngularVelocity = TGCVector3.Empty.ToBulletVector3();
                Capsula.ApplyCentralImpulse(new TGCVector3(0, 2f, 0).ToBulletVector3());
            }

            if (TocandoPiso()) {
                jumping = false;
            }
        }

        private bool TocandoPiso() {
            var vel = FastMath.Abs(Capsula.LinearVelocity.Y);
            return vel < 0.01f;
        }

        private bool jumping = false;

        public void Update(GameModel game) {
            if (!onPause)
            {
                MoverCapsula(game.ElapsedTime, game.Input);
                Posicion = Capsula.CenterOfMassPosition.ToTGCVector3();
                CamaraInterna.PositionEye = Posicion;

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

        public void AgregarItem(ECrafteable tipo) {
            Inventario.AgregarItem(tipo);
        }

        public void GastarItem(ERecolectable tipo) {
            Inventario.Sacar(tipo);
        }

        public void CraftearItem(ICrafteable item) {
            AgregarItem(item.Tipo);
            foreach(var m in item.Materiales) {
                for (int i = 0; i < m.Value; i++)
                    GastarItem(m.Key);
            }
        }

        private void ActualizarOxigeno(GameModel game)
        {
            Oxigeno = !Sumergido && Oxigeno < HUD.BarraOxigeno.ValorMaximo ?
                Oxigeno += 14f * game.ElapsedTime :
                Oxigeno -= 7f * game.ElapsedTime;
        }

        private bool _murio = false;
        private void BloquearCamara(TgcFpsCamera camara) {
            if (!_murio)
            {
                camara.Lock();
            }
        }

        public void Lock() {
            BloquearCamara(CamaraInterna);
        }

        public int CuantosTiene(ERecolectable material) {
            return Inventario.CuantosTiene(material);
        }
        
        public void Render() {
            if (EstaVivo)
            {
                Arma.Render();
                HUD.Render();
            }
        }

        public bool Sumergido => Posicion.Y < 2800f;
        public bool RozandoSuperficie => Posicion.Y <= 2900f && Posicion.Y >= 2700f;

    }
}
