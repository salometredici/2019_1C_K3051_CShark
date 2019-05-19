using TGC.Core.Mathematica;
using CShark.Model;
using TGC.Core.Input;
using CShark.Items;
using BulletSharp;
using CShark.Terreno;
using TGC.Core.BulletPhysics;
using CShark.Utils;
using BulletSharp.Math;
using TGC.Core.Collision;
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
        private UbicacionPlayer Ubicacion;
        
        public bool onPause;
        public bool EstaVivo => Vida > 0 && Oxigeno > 0;
        public bool Sumergido => Posicion.Y < 18000f;
        public bool RozandoSuperficie => Posicion.Y <= 18100f && Posicion.Y >= 17900f;
        public bool Saltando = false;

        public TgcFpsCamera CamaraInterna;
        public TgcD3dInput Input;
        public TGCVector3 MoveVector;
        public RigidBody Capsula;
        private RayoProximidad RayoProximidad;
        private InputHandler InputHandler;

        public Player(TGCVector3 posicion, int vidaInicial, int oxigenoInicial, TgcD3dInput input) {
            Inventario = new Inventario();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            HUD = new HUD(Vida, Oxigeno);
            Input = input;
            InputHandler = new InputHandler(this);
            CamaraInterna = new TgcFpsCamera(input, this);
            InicializarVariables(vidaInicial, oxigenoInicial);
            CrearCapsula();
            Ubicacion = UbicacionPlayer.Superficie;
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
            Capsula = BulletRigidBodyFactory.Instance.CreateCapsule(300, 500, Posicion, 5, true);
            Capsula.SetDamping(0.1f, 0f);
            Capsula.Restitution = 0.1f;
            Capsula.Friction = 0f; //la friccion hago que dependa de las superficies, no del player..
            Capsula.AngularFactor = new Vector3(0, 1, 0);
            Mapa.Instancia.AgregarBody(Capsula);
        }

        float Velocidad = 5000f;
        float Salto = 10f;
        float Flote = 20f;

        public void MoverCapsula(float x, float y, float z) {
            Capsula.LinearVelocity += Velocidad * new Vector3(x , 0, z);
        }

        public void Flotar(int sentido) {
            Impulsar(new Vector3(0, Flote * sentido, 0));
        }

        private void Impulsar(Vector3 impulso) {
            Capsula.ActivationState = ActivationState.ActiveTag;
            Capsula.ApplyCentralImpulse(impulso);
        }

        public void Saltar() {
            if (!Saltando) {
                Saltando = true;
                Impulsar(new Vector3(0, Velocidad * Salto, 0));
            }
        }

        private bool TocandoPiso() {
            var vel = FastMath.Abs(Capsula.LinearVelocity.Y);
            return vel < 0.01f;
        }

        private void Detenerse() {
            Capsula.LinearVelocity -= new Vector3(Capsula.LinearVelocity.X, 0, Capsula.LinearVelocity.Z);
        }

        public void Update(GameModel game) {
            time += game.ElapsedTime;
            if (!onPause)
            {
                Detenerse();
                InputHandler.Update();
                Posicion = Capsula.CenterOfMassPosition.ToTGCVector3();
                CamaraInterna.PositionEye = Posicion;
                RayoProximidad.Update(CamaraInterna, Posicion);
                ActualizarOxigeno(game);
                CheckearUbicacion();
                if (EstaVivo) {
                    Arma.Update(game);
                    HUD.Update(Vida, Oxigeno, game.ElapsedTime);
                }
                else {
                    _murio = true;
                    BloquearCamara(CamaraInterna);
                }
                if (TocandoPiso()) {
                    Saltando = false;
                }
            }
        }

        private void CheckearUbicacion() {
            var ubic = Ubicacion;
            Ubicacion = Sumergido ? UbicacionPlayer.Sumergido : UbicacionPlayer.Superficie;
            if (ubic != Ubicacion)
                Mapa.Instancia.CambiarEfecto(Sumergido);
        }

        public void Recoger(Recolectable item) {
            Inventario.Agregar(item);
            //HUD.PopMensaje(item.Tipo);
        }

        public void AgregarItem(ECrafteable tipo) {
            Inventario.AgregarItem(tipo);
        }

        public void GastarItem(ERecolectable tipo) {
            Inventario.Sacar(tipo);
        }

        public void CraftearItem(ICrafteable item) {
            AgregarItem(item.Tipo);
            foreach (var m in item.Materiales) {
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

    public enum UbicacionPlayer
    {
        Sumergido,
        Superficie
    }
}