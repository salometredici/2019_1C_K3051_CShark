using BulletSharp;
using CShark.Fisica;
using CShark.Fisica.Colisiones;
using CShark.Items;
using CShark.Jugador;
using CShark.Model;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        private SkyBox Skybox;
        public ColisionesTerreno Colisiones;

        private Terrain FondoDelMar;
        private Isla Isla;
        private TgcScene Rocas;
        private TgcScene Extras;
        public Superficie Superficie;

        private static Mapa instancia;

        public static Mapa Instancia {
            get {
                if (instancia == null)
                    instancia = new Mapa();
                return instancia;
            }
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Centro.Y;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;
        public float AlturaMar => 2800f;

        private Mapa() {
            FondoDelMar = new Terrain();
            CargarTerreno(FondoDelMar, @"Mapa\Textures\hm.jpg", @"Mapa\Textures\seafloor.jpg", 30000 / 512f, 2f, TGCVector3.Empty);
            Centro = FondoDelMar.Center;
            Skybox = new SkyBox(Centro);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Superficie = new Superficie();
            Superficie.CargarTerrains();
            Colisiones = new ColisionesTerreno
            {
                OlasRB = BulletRigidBodyFactory.Instance.CreateSurfaceFromHeighMap(Superficie.Terrain.getData())
            };
            Colisiones.Init(FondoDelMar.getData());
            Isla = new Isla(this);
            CargarParedes();
        }

        private void CargarParedes() {
            foreach (var face in Skybox.FacesToRender) {
                var size = face.BoundingBox.calculateSize();
                var position = face.BoundingBox.Position;
                var body = BulletRigidBodyFactory.Instance.CreateBox(size, 0, position, 0, 0, 0, 1, false);
                AgregarBody(body);
            }
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
            Superficie.Update(elapsedTime);
            Isla.Update(game);
            Colisiones.Update();
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
            foreach(var mesh in scene.Meshes)
            {
                CargarRigidBodyFromMesh(mesh);
            }
        }

        public void CargarRigidBodyFromMesh(TgcMesh mesh)
        {
            var rigidBody = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(mesh);
            AgregarBody(rigidBody);
        }

        public void Render(Player player) {
            FondoDelMar.Render();
            Skybox.Render();
            Isla.Render();
            Rocas.RenderAll();
            Extras.RenderAll();
            Superficie.Render();
        }

        public void Dispose() {
            FondoDelMar.Dispose();
            Skybox.Dispose();
            Isla.Dispose();
            Rocas.DisposeAll();
            Extras.DisposeAll();
            Superficie.Dispose();
        }

        private void CargarTerreno(Terrain terrain, string heightMapDir, string textureDir, float xz, float y, TGCVector3 position) {
            terrain.AlphaBlendEnable = true;
            terrain.loadHeightmap(mediaDir + heightMapDir, xz, y, position);
            terrain.loadTexture(mediaDir + textureDir);
        }
    }
}
