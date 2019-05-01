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

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        private List<Pez> Peces;
        public static Tiburon Tiburon;
        private TgcScene Spawns;

        public FaunaManager(TgcScene spawns) {
            Spawns = spawns;
        }

        public void Initialize() {
            Peces = new List<Pez>();
            foreach (var mesh in Spawns.Meshes)
                Spawnear(mesh.Name, mesh.BoundingBox.Position);
            Spawns = null;
        }

        private void Spawnear(string tipo, TGCVector3 posicion) {
            if (tipo.Contains("Tiburon"))
                Tiburon = new Tiburon(posicion);
            else if (tipo.Contains("Payaso"))
                Peces.Add(new PezPayaso(posicion));
            else if (tipo.Contains("Azul"))
                Peces.Add(new PezAzul(posicion));
            else if (tipo.Contains("Betta"))
                Peces.Add(new PezBetta(posicion));
            else if (tipo.Contains("Tropical"))
                Peces.Add(new PezTropical(tipo.Last(), posicion));
        }

        public void Update(GameModel game) {
            Peces.ForEach(pez => pez.Update(game.ElapsedTime));
            Tiburon.Update(game.ElapsedTime);
        }

        public void Render(GameModel game) {
            Peces.ForEach(pez => pez.Render());
            Tiburon.Render();
        }   
    }
}
