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
        public int Arpones = 0;
        public int Tanques = 0;
        public int Vidas = 0;

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

        public void AgregarItem(Crafteable tipo) {
            switch (tipo) {
                case Crafteable.Arpon:
                    Arpones++;
                    break;
                case Crafteable.Tanque:
                    Tanques++;
                    break;
                case Crafteable.Vida:
                    Vidas++;
                    break;
            }            
        }

        public void GastarItem(Crafteable tipo) {
            switch (tipo) {
                case Crafteable.Arpon:
                    Arpones--;
                    break;
                case Crafteable.Tanque:
                    Tanques--;
                    break;
                case Crafteable.Vida:
                    Vidas--;
                    break;
            }
        }
    }
}
