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
using TGC.Core.Terrain;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        private SkyBox Skybox;
        private TgcSimpleTerrain Terreno;
        private ColisionesTerreno Colisiones;
        private Isla Isla;
        private TgcScene Rocas;
        private TgcScene Extras;
        public Superficie Superficie;

        public static Mapa Instancia { get; } = new Mapa();

        private Mapa() { }
              
        public void CargarSkybox() {
            Centro = Terreno.Center;
            Skybox = new SkyBox(Centro);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Isla = new Isla(this);
        }

        public void CargarTerreno() {
            var tamañoHM = 100f;
            Terreno = new TgcSimpleTerrain();
            CargarTerreno(Terreno, @"Mapa\Textures\fondo.png", @"Mapa\Textures\seafloor.jpg", 60000 / tamañoHM, 1f, TGCVector3.Empty);
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(Terreno.getData());
        }

        public void CargarSuperficie() {
            Superficie = new Superficie();
            Superficie.CargarTerrains();
            /*var olas = BulletRigidBodyFactory.Instance.CreateSurfaceFromHeighMap(Superficie.Terrain.getData());
            AgregarBody(olas);
            Colisiones.OlasRB = olas;*/
            //no tiene sentido porque la posicion de las olas la hago desde el shader. por ahora 
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
            Terreno.Render();
            Skybox.Render();
            Isla.Render();
            Rocas.RenderAll();
            Extras.RenderAll();
            Superficie.Render();
        }

        public void Dispose() {
            Terreno.Dispose();
            Skybox.Dispose();
            Isla.Dispose();
            Rocas.DisposeAll();
            Extras.DisposeAll();
            Superficie.Dispose();
        }

        private void CargarTerreno(TgcSimpleTerrain terreno, string heightMapDir, string textureDir, float xz, float y, TGCVector3 position) {
            terreno.AlphaBlendEnable = true;
            terreno.loadHeightmap(mediaDir + heightMapDir, xz, y, position);
            terreno.loadTexture(mediaDir + textureDir);
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
