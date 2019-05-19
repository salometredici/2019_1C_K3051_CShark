using CShark.Items;
using CShark.Items.Recolectables;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Managers
{
    public class RecolectablesManager : IManager
    {
        private List<IRecolectable> Recolectables;
        private TgcScene Spawns;

        public RecolectablesManager(TgcScene spawns) {
            Spawns = spawns;
        }

        public void Initialize(TgcScene spawns) {
            Recolectables = new List<IRecolectable>();
            foreach(var mesh in spawns.Meshes)
                Spawnear(mesh.BoundingBox.Position);
            Spawns.DisposeAll();
        }

        private void Spawnear(TGCVector3 posicion) {
            var item = RandomItem(posicion);
            Recolectables.Add(item);
        }

        private ERecolectable RandomTipo() {
            Array values = Enum.GetValues(typeof(ERecolectable));
            Random random = new Random();
            return (ERecolectable)values.GetValue(random.Next(values.Length));
        }

        private IRecolectable RandomItem(TGCVector3 posicion) {
            var tipo = RandomTipo();
            switch (tipo) {
                case ERecolectable.Pila: return new Pila(posicion);
                case ERecolectable.Burbuja: return new Burbuja(posicion);
                case ERecolectable.Chip: return new Chip(posicion);
                case ERecolectable.Wumpa: return new Wumpa(posicion);
                default: return new Pila(posicion);
            }
        }

        public void Render(GameModel game) {
            Recolectables.ForEach(r => r.Render(game));
        }

        public void Update(GameModel game) {
            Recolectables.ForEach(r => r.Update(game));
        }

        public void Initialize() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            Recolectables.ForEach(r => r.Dispose());
        }
    }
}
