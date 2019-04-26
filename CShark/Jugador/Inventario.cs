using CShark.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Jugador
{
    public class Inventario
    {
        private List<IRecolectable> Items;

        public Inventario() {
            Items = new List<IRecolectable>();
        }

        public void Agregar(IRecolectable item) {
            Items.Add(item);
        }

        public void Sacar(IRecolectable item) {
            Items.Remove(item);
        }

        public bool Tiene(IRecolectable item) {
            return Items.Contains(item);
        }
    }
}
