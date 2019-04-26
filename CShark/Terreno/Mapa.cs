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

        private TgcSkyBox Skybox;
        private Superficie Superficie;
        private TgcScene Vegetacion;
        private TgcSimpleTerrain Terreno;

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
            CargarSkyBox();
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Centro = Skybox.Center;
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Terreno.Position.Y + 40f;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;

        public void Update() {
            //Superficie.Update();
        }

        public void Render() {
            Terreno.Render();
            Skybox.Render();
            Vegetacion.RenderAll();
            //Superficie.Render();
        }

        public void Dispose() {
            Terreno.Dispose();
            Skybox.Dispose();
            Vegetacion.DisposeAll();
            Superficie.Dispose();
        }

        private void CargarTerreno() {
            var loader = new TgcSceneLoader();
            Terreno = new TgcSimpleTerrain();
            var media = Game.Default.MediaDirectory;
            Terreno.loadHeightmap(media + "Heightmaps\\heightmap.jpg", 100, 1.8f, TGCVector3.Empty);
            Terreno.loadTexture(media + "Textures\\UnderwaterSkybox\\seafloor.jpg");
            Vegetacion = loader.loadSceneFromFile(media + "vegetation-TgcScene.xml");
        }

        private void CargarSkyBox() {
            Skybox = new TgcSkyBox();
            Skybox.Center = Terreno.Center;
            Skybox.Size = new TGCVector3(10000, 5000, 10000);
            var texturesPath = Game.Default.MediaDirectory + "Textures\\UnderwaterSkybox\\";
            Skybox.setFaceTexture(SkyFaces.Up, texturesPath + "blue-texture.png");
            Skybox.setFaceTexture(SkyFaces.Down, texturesPath + "seafloor.jpg");
            Skybox.setFaceTexture(SkyFaces.Left, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Right, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Front, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Back, texturesPath + "side.jpg");
            Skybox.SkyEpsilon = 50f;
            Skybox.Init();
        }
    }
}
