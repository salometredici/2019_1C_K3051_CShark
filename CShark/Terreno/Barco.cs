using CShark.Jugador;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Terreno
{
    public class Barco : IDisposable
    {
        private TgcMesh Mesh;
        private MesaCrafteo Mesa;

        public Barco() {
            var loader = new TgcSceneLoader();
            var barco = loader.loadSceneFromFile(Game.Default.MediaDirectory + "bot-TgcScene.xml").Meshes[0];
            barco.Position = new TGCVector3(2000f, 2950f, 0);
            barco.Scale = new TGCVector3(0.15f, 0.15f, 0.15f);
            Mesh = barco;
            Mesa = new MesaCrafteo(barco.Position + new TGCVector3(0, 100, 0));
        }

        public void Update(GameModel game) {
            Mesa.Update(game);
        }

        public void Render() {
            Mesh.Render();
            Mesa.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
            Mesa.Dispose();
        }
    }
}
