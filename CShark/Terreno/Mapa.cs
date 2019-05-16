using BulletSharp;
using CShark.Fisica.Colisiones;
using CShark.Jugador;
using CShark.Model;
using CShark.Utilidades;
using CShark.Utils;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        private SkyBox Skybox;
        private Suelo Suelo;
        private ColisionesTerreno Colisiones;
        private Barco Barco;
        private TgcScene Rocas;
        private TgcScene Extras;
        public Superficie Superficie;
        private Sol Sol;

        public static Mapa Instancia { get; } = new Mapa();

        private Mapa() { }
              
        public void CargarSkybox() {
            Centro = Suelo.Center;
            Skybox = new SkyBox(Centro);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Sol = new Sol(Centro + new TGCVector3(0, 50000, 0));
        }

        public void CargarBarco(TgcMesh mesh) {
            Barco = new Barco(mesh);
        }

        public void CargarTerreno() {
            Suelo = new Suelo();
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(Suelo.GetData());
        }

        public void CargarSuperficie() {
            Superficie = new Superficie();
            Superficie.CargarTerrains();
        }

        public void CargarParedes(TgcScene paredes) {
            foreach (var face in paredes.Meshes) {
                var size = face.BoundingBox.calculateSize();
                var position = face.BoundingBox.Position;
                var body = BulletRigidBodyFactory.Instance.CreateBox(size, 0, position, 0, 0, 0, 1, false);
                AgregarBody(body);
            }
            paredes.DisposeAll();
        }


        public void CargarRocas(TgcScene rocas) {
            Rocas = rocas;
            CargarRigidBodiesFromScene(Rocas);
        }

        public void CargarExtras(TgcScene extras) {
            Extras = extras;
            CargarRigidBodiesFromScene(Extras);
        }

        public void Update(float elapsedTime, GameModel game) {
            if (game.Player.Posicion.Y > Superficie.Altura) {
                Colisiones.CambiarGravedad(Constants.StandardGravity);
            } else {
                Colisiones.CambiarGravedad(Constants.UnderWaterGravity);
            }
            Suelo.Update(game.Player.CamaraInterna.PositionEye);
            Superficie.Update(elapsedTime);
            Sol.Update(elapsedTime);
            Barco.Update(game);
            Colisiones.Update(elapsedTime);
        }

        public void AgregarBody(RigidBody body) {
            Colisiones.AgregarBody(body);
        }

        public void SacarBody(RigidBody body) {
            Colisiones.SacarBody(body);
        }

        public bool Colisionan(CollisionObject ob1, CollisionObject ob2) {
            return Colisiones.Colisionan(ob1, ob2);
        }

        public void CargarRigidBodiesFromScene(TgcScene scene)
        {
            //esto me baja de 700fps a 80 :)))))) hay algunas rocas y eso que tienen 
            //bocha de poligonos, usar esferas o algo mas simple..
            /*foreach(var mesh in scene.Meshes)
            {
                CargarRigidBodyFromMesh(mesh);
            }*/
        }

        public void CargarRigidBodyFromMesh(TgcMesh mesh)
        {
            var rigidBody = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(mesh);
            AgregarBody(rigidBody);
        }

        public void Render(Player player) {
            Suelo.Render();
            Skybox.Render();
            Barco.Render();
            Rocas.RenderAll();
            Extras.RenderAll();
            Superficie.Render();
            Sol.Render();
        }

        public void Dispose() {
            Suelo.Dispose();
            Skybox.Dispose();
            Barco.Dispose();
            Rocas.DisposeAll();
            Extras.DisposeAll();
            Superficie.Dispose();
            Sol.Dispose();
            MeshLoader.Instance.Dispose();
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Centro.Y;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;
        public float AlturaMar => 2800f;
    }
}
