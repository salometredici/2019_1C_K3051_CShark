using CShark.Items;
using CShark.Items.Recolectables;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Managers
{
    public class RecolectablesManager : IManager
    {
        private List<IRecolectable> Recolectables;

        public RecolectablesManager() {
            Recolectables = new List<IRecolectable>();
        }

        public void Initialize() {
            Recolectables.Add(new Wumpa(new TGCVector3(100, 500, 100)));
            Recolectables.Add(new Wumpa(new TGCVector3(0, 1000, 0)));
            Recolectables.Add(new Burbuja(new TGCVector3(250, 400, 0)));
            Recolectables.Add(new Burbuja(new TGCVector3(350, 400, 100)));
            Recolectables.Add(new Burbuja(new TGCVector3(-100, 400, -50)));
            Recolectables.Add(new Chip(new TGCVector3(500, 400, -300)));
            Recolectables.Add(new Bateria(new TGCVector3(-400, 400, -100)));
        }

        public void Render(GameModel game) {
            Recolectables.ForEach(r => r.Render());
        }

        public void Update(GameModel game) {
            Recolectables.ForEach(r => r.Update(game));
        }
    }
}
