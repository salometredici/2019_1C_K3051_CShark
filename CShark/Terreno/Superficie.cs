using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using TGC.Core.Shaders;

namespace CShark.Terreno
{
    public class Superficie
    {
        private List<TgcMesh> Olas;
        private TgcMesh Mesh;

        private readonly int AnchoGrilla = 20;
        private readonly int AltoGrilla = 20;
        private readonly int AlturaMar = 800; //fruta
        private readonly TGCVector3 Tamaño;

        private readonly float VelocidadOlas = 100f;

        private readonly TGCVector3 Spawn;
        
        public Superficie() {
            var loader = new TgcSceneLoader();
            Spawn = new TGCVector3(-3200, AlturaMar, -3200);
            Mesh = loader.loadSceneFromFile(Game.Default.MediaDirectory + "Olas-TgcScene.xml").Meshes[0];
            Tamaño = Mesh.BoundingBox.calculateSize();
            CargarOlas();
        }

        public void Update(float elapsedTime) {
            foreach(var ola in Olas)
            {
                ola.Position += new TGCVector3(VelocidadOlas * elapsedTime, 0, 0);
                if (ola.Position.X > Tamaño.X * AnchoGrilla)
                    ola.Position = new TGCVector3(Spawn.X, Spawn.Y, ola.Position.Z);
            }
        }

        public void Render() {
            Olas.ForEach(o => o.Render());
        }

        public void Dispose() {
            Olas.ForEach(o => o.Dispose());
        }

        private void CargarOlas() {
            Olas = new List<TgcMesh>();
            for (int i = 0; i < AnchoGrilla; i++)
            {
                for (int j = 0; j < AltoGrilla; j++)
                {
                    var centro = new TGCVector3(Spawn.X + i * (Tamaño.X - 2), Spawn.Y, Spawn.Z + j * (Tamaño.Z - 1));
                    var ola = Mesh.clone("ola");
                    ola.Position = centro;
                    Olas.Add(ola);
                }
                
            }
        }
    }
}
