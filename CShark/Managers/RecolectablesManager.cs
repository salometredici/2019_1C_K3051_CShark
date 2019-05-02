using CShark.Items;
using CShark.Items.Recolectables;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Managers
{
    public class RecolectablesManager : IManager
    {
        private List<IRecolectable> Recolectables;

        public RecolectablesManager() {
            Recolectables = new List<IRecolectable>();
        }

        public void Initialize() {
            /*Recolectables.Add(new Wumpa(100, 500, 100));
            Recolectables.Add(new Wumpa(0, 1000, 0));
            Recolectables.Add(new Oxigeno(250, 100, 0));
            Recolectables.Add(new Oxigeno(350, 100, 100));
            Recolectables.Add(new Oxigeno(-100, 100, -50));
            Recolectables.Add(new Medkit(500, 50, -300));
            Recolectables.Add(new Medkit(-400, 50, -100));*/
        }

        public void Render(GameModel game) {
            Recolectables.ForEach(r => r.Render());
        }

        public void Update(GameModel game) {
            Recolectables.ForEach(r => r.Update(game.Player));
        }
    }
}
