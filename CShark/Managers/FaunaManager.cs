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

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        private List<Pez> Peces;
        public static Tiburon Tiburon;

        public FaunaManager() {
            Initialize();
        }

        public void Initialize() {
            Peces = new List<Pez>();
            Tiburon = new Tiburon(100, 500, 100);
            CargarPez(new PezPayaso(0, 0, 0));
            CargarPez(new PezAzul(100, 0, 100));
            CargarPez(new PezBetta(0, 0, 300));
            CargarPez(new PezTropical(1, 0, 100, 0));
            CargarPez(new PezTropical(2, 0, 200, 0));
            CargarPez(new PezTropical(3, 0, 300, 0));
            CargarPez(new PezTropical(4, 0, 400, 0));
            CargarPez(new PezTropical(5, 0, 500, 0));
            CargarPez(new PezTropical(6, 0, 600, 0));
        }

        public void Update(GameModel game) {
            Peces.ForEach(pez => pez.Update(game.ElapsedTime));
            Tiburon.Update(game.ElapsedTime);
        }

        public void Render(GameModel game) {
            Peces.ForEach(pez => pez.Render());
            Tiburon.Render();
        }

        private void CargarPez(Pez pez) {
            Peces.Add(pez);
        }        
    }
}
