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
using TGC.Core.Collision;
using TGC.Core.Geometry;
using System.Drawing;
using TGC.Core.Text;
using TGC.Core.Direct3D;
using CShark.Jugador.Camara;

namespace CShark.Jugador
{
    public class Player
    {
        public float Vida;
        public float Oxigeno;
        private Inventario Inventario;
        private HUD HUD;
        private Arma Arma;
        public TGCVector3 Posicion;
        public bool EstaVivo => Vida > 0 && Oxigeno > 0;
        public bool onPause;
        private bool jumping = false;

        public TgcFpsCamera CamaraInterna;
        public TgcD3dInput Input;
        public TGCVector3 MoveVector;
        private RigidBody Capsula;
        private RayoProximidad RayoProximidad;

        public bool Sumergido => Posicion.Y < 2800f;
        public bool RozandoSuperficie => Posicion.Y <= 2900f && Posicion.Y >= 2700f;

        public Player(TGCVector3 posicion, int vidaInicial, int oxigenoInicial, TgcD3dInput input) {
            Posicion = posicion;            
            Input = input;
            CamaraInterna = new TgcFpsCamera(input, this);
            InicializarVariables(vidaInicial, oxigenoInicial);
            CrearCapsula();            
        }

        private void InicializarVariables(int vida, int oxigeno)
        {
            Inventario = new Inventario();
            Vida = vida;
            Oxigeno = oxigeno;
            HUD = new HUD(Vida, Oxigeno);
            Arma = new Crossbow();
            onPause = false;
            RayoProximidad = new RayoProximidad();
        }

        private void CrearCapsula()
        {
            Capsula = BulletRigidBodyFactory.Instance.CreateCapsule(300, 500, Posicion, 5, false);
            Capsula.SetDamping(0.1f, 0f);
            Capsula.Restitution = 0.1f;
            Capsula.Friction = 0.3f;
            Mapa.Instancia.AgregarBody(Capsula);
        }

        private void MoverCapsula(float elapsedTime, TgcD3dInput input) {
            var strength = 50.30f;

            if (input.keyDown(Key.W)) {
                var cos = FastMath.Cos(CamaraInterna.leftrightRot);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot);
                UpdateCapsula(-strength * new Vector3(sin, 0, cos));
            }

            if (input.keyDown(Key.S)) {
                var cos = FastMath.Cos(CamaraInterna.leftrightRot);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot);
                UpdateCapsula(strength * new Vector3(sin, 0, cos));
            }

            if (input.keyDown(Key.D)) {
                var cos = FastMath.Cos(CamaraInterna.leftrightRot + FastMath.PI / 2);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot + FastMath.PI / 2);
                UpdateCapsula(new Vector3(20f + sin, 0, 20f + cos));
            }

            if (input.keyDown(Key.A)) {
                var cos = FastMath.Cos(CamaraInterna.leftrightRot + FastMath.PI / 2);
                var sin = FastMath.Sin(CamaraInterna.leftrightRot + FastMath.PI / 2);
                UpdateCapsula(new Vector3(20f + cos, 0, 20f  + sin));
            }

            if (input.keyPressed(Key.Space) && !jumping) {
                if (RozandoSuperficie || !Sumergido) {
                    jumping = true;
                    UpdateCapsula(new Vector3(0, 50 * strength, 0));
                }
                else
                {
                    UpdateCapsula(new Vector3(0, 100 * strength, 0));
                }
            }

            if (input.keyPressed(Key.X))
            {
                UpdateCapsula(new Vector3(0, -100 * strength, 0));
            }

            if (input.keyPressed(Key.F))
            {
                Capsula.LinearVelocity = Vector3.Zero;
            }

            //nadar lentamente hacia arriba
            if (input.keyDown(Key.Space) && Sumergido) {
                UpdateCapsula(new Vector3(0, 50f, 0));
            }

            if (TocandoPiso()) {
                jumping = false;
            }
        }

        private void UpdateCapsula(Vector3 direccion)
        {
            Capsula.ActivationState = ActivationState.ActiveTag;
            Capsula.AngularVelocity = Vector3.Zero;
            Capsula.ApplyCentralImpulse(direccion);
        }

        private bool TocandoPiso() {
            var vel = FastMath.Abs(Capsula.LinearVelocity.Y);
            return vel < 0.01f;
        }
        
        public void Update(GameModel game) {
            time += game.ElapsedTime;
            if (!onPause)
            {
                MoverCapsula(game.ElapsedTime, game.Input);
                Posicion = Capsula.CenterOfMassPosition.ToTGCVector3();
                CamaraInterna.PositionEye = Posicion;
                RayoProximidad.Update(CamaraInterna, Posicion);
                ActualizarOxigeno(game);
                if (EstaVivo)
                {
                    Arma.Update(game);
                    HUD.Update(Vida, Oxigeno, game.ElapsedTime);
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
            HUD.PopMensaje(item.Tipo);
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

        public bool PuedeRecoger(IRecolectable recolectable) {
            return EstaMirando(recolectable) && EstaCerca(recolectable);
        }

        private bool EstaCerca(IRecolectable recolectable) {
            return TgcCollisionUtils.testPointSphere(recolectable.EsferaCercania, Posicion);
        }
        
        private bool EstaMirando(IRecolectable recolectable) {
            return RayoProximidad.Intersecta(recolectable.Box);
        }

        public int CuantosTiene(ERecolectable material) {
            return Inventario.CuantosTiene(material);
        }

        private float time = 0;

        public void Render() {
            if (EstaVivo)
            {
                RayoProximidad.Render();
                Arma.Render();
                HUD.Render();
            }
        }

    }
}
