using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using CShark.NPCs.Peces;
using CShark.Model;
using CShark.NPCs.Enemigos;
using TGC.Core.Mathematica;
using CShark.Animales;

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        private List<Animal> Animales;
        public static Tiburon Tiburon;
        private TgcScene Spawns;

        public FaunaManager(TgcScene spawns) {
            Spawns = spawns;
        }

        public void Initialize() {
            Animales = new List<Animal>();
            foreach (var mesh in Spawns.Meshes)
                Spawnear(mesh.Name, mesh.BoundingBox.Position);
            Spawns.DisposeAll();
        }

        private void Spawnear(string tipo, TGCVector3 posicion) {
            if (tipo.Contains("Tiburon")) {
                Tiburon = new Tiburon(posicion);
                Animales.Add(Tiburon);
            }
            else if (tipo.Contains("Payaso"))
                Animales.Add(new PezPayaso(posicion));
            else if (tipo.Contains("Azul"))
                Animales.Add(new PezAzul(posicion));
            else if (tipo.Contains("Betta"))
                Animales.Add(new PezBetta(posicion));
            else if (tipo.Contains("Tropical"))
                Animales.Add(new PezTropical(tipo.Last(), posicion));
        }

        public void Update(GameModel game) {
            Animales.ForEach(animal => animal.Update(game.ElapsedTime));
        }

        public void Render(GameModel game) {
            Animales.ForEach(animal => animal.Render());
        }

        public void Dispose() {
            Animales.ForEach(a => a.Dispose());
            Tiburon.Dispose();
        }
    }
}
