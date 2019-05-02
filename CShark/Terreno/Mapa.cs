using BulletSharp;
using CShark.Fisica.Colisiones;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using static TGC.Core.Terrain.TgcSkyBox;

namespace CShark.Terreno
{
    public class Mapa
    {
        private TGCBox Box;
        public TGCVector3 Centro;

        private SkyBox Skybox;
        private Isla Isla; //Es la misma del TGC.Viewer

        //private Superficie Superficie;
        private TgcScene Vegetacion;
        private TgcSimpleTerrain Terreno;
        private ColisionesTerreno Colisiones;

        private TgcScene Rocas;
        private TgcScene Extras;

        private static Mapa instancia;

        public static Mapa Instancia {
            get {
                if (instancia == null)
                    instancia = new Mapa();
                return instancia;
            }
        }

        private Mapa() {
            CargarTerreno();
            Skybox = new SkyBox(Terreno.Center);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Centro = Terreno.Center;
            Isla = new Isla();
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(Terreno.getData());
        }

        public void CargarRocas(TgcScene rocas) {
            Rocas = rocas;
        }

        public void CargarExtras(TgcScene extras) {
            Extras = extras;
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Centro.Y;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;

        public void Update() {
            //Superficie.Update();
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

        public void Render(TGCVector3 playerPosition) {
            Terreno.Render();
            Skybox.Render(playerPosition);
            Isla.Render();
            //Vegetacion.RenderAll();
            Rocas.RenderAll();
            Extras.RenderAll();
            //Superficie.Render();
        }

        public void Dispose() {
            Terreno.Dispose();
            Skybox.Dispose();
            Isla.Dispose();
            //Vegetacion.DisposeAll();
            Rocas.DisposeAll();
            Extras.DisposeAll();
            //Superficie.Dispose();
        }

        private void CargarTerreno() {
            var loader = new TgcSceneLoader();
            Terreno = new TgcSimpleTerrain();
            var media = Game.Default.MediaDirectory;
            var tamañoHM = 512f;
            var tamañoTerreno = 10000;
            var xz = tamañoTerreno / tamañoHM;
            var y = 1f;
            Terreno.loadHeightmap(media + @"Mapa\Textures\hm.jpg", xz, y, TGCVector3.Empty);
            Terreno.loadTexture(media + @"Mapa\Textures\arena.png");
            //Vegetacion = loader.loadSceneFromFile(media + "vegetation-TgcScene.xml");
        }

    }
}
