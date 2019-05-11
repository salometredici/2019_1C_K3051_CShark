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
            for (int i = 0; i < 5; i++)
                Recolectables.Add(new Wumpa(new TGCVector3(i * 300, 300, 500)));
            for (int i = 0; i < 5; i++)
                Recolectables.Add(new Chip(new TGCVector3(i * 400, 800, 0)));
            for (int i = 0; i < 5; i++)
                Recolectables.Add(new Burbuja(new TGCVector3(600, 1000, i * 600)));
            for (int i = 0; i < 5; i++)
                Recolectables.Add(new Bateria(new TGCVector3(400, 400, i * 400)));
        }

        public void Render(GameModel game) {
            Recolectables.ForEach(r => r.Render(game));
        }

        public void Update(GameModel game) {
            Recolectables.ForEach(r => r.Update(game));
        }
    }
}
