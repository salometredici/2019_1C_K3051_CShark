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

        //private Superficie Superficie;
        private TgcScene Vegetacion;
        private TgcSimpleTerrain Terreno;
        private ColisionesTerreno Colisiones;

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
            Centro = Skybox.Center;
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(Terreno.getData());
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;

        public float YMin => Terreno.Position.Y + 1000f; //ESTO VER DESPUES

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

        public void Render(TGCVector3 playerPosition) {
            Terreno.Render();
            Skybox.Render(playerPosition);
            Vegetacion.RenderAll();
            //Superficie.Render();
        }

        public void Dispose() {
            Terreno.Dispose();
            Skybox.Dispose();
            Vegetacion.DisposeAll();
            //Superficie.Dispose();
        }

        private void CargarTerreno() {
            var loader = new TgcSceneLoader();
            Terreno = new TgcSimpleTerrain();
            var media = Game.Default.MediaDirectory;
            Terreno.loadHeightmap(media + "Heightmaps\\heightmap.jpg", 100, 1.8f, TGCVector3.Empty);
            Terreno.loadTexture(media + "Textures\\UnderwaterSkybox\\seafloor.jpg");
            Vegetacion = loader.loadSceneFromFile(media + "vegetation-TgcScene.xml");
        }

    }
}
