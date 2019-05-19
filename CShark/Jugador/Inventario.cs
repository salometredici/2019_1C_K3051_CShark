using CShark.Items;
using CShark.Items.Crafteables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Jugador
{
    public class Inventario
    {
        public int Arpones = 0;
        public int Oxigenos = 0;
        public int Medkits = 0;
        public int Corales = 0;
        public int Wumpas = 0;
        public int Peces = 0;
        public int Oro = 0;
        public int Plata = 0;
        public int Hierro = 0;
        public int Pilas = 0;
        public int Chips = 0;
        public int Burbujas = 0;

        private List<IRecolectable> Items;

        public Inventario() {
            Items = new List<IRecolectable>();
        }

        public void Agregar(IRecolectable item) {
            Items.Add(item);
        }

        public void Sacar(ERecolectable tipo) {
            var item = Items.FirstOrDefault(i => i.Tipo == tipo);
            if (item != null)
                Items.Remove(item);
        }

        public bool Tiene(IRecolectable item) {
            return Items.Contains(item);
        }

        public void AgregarItem(ECrafteable tipo) {
            switch (tipo) {
                case ECrafteable.Arpon:
                    Arpones++;
                    break;
                case ECrafteable.Oxigeno:
                    Oxigenos++;
                    break;
                case ECrafteable.Medkit:
                    Medkits++;
                    break;
                case ECrafteable.Oro:
                    Oro++;
                    break;
            }            
        }

        public void GastarItem(ECrafteable tipo) {
            switch (tipo) {
                case ECrafteable.Arpon:
                    Arpones--;
                    break;
                case ECrafteable.Oxigeno:
                    Oxigenos--;
                    break;
                case ECrafteable.Medkit:
                    Medkits--;
                    break;
                case ECrafteable.Oro:
                    Oro--;
                    break;
            }
        }

        public int CuantosTiene(ERecolectable material) {
            return Items.Where(i => i.Tipo.Equals(material)).Count();
        }
    }
}
