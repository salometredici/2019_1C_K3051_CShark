using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Jugador;

namespace CShark.Items.Crafteables
{
    public class Oxigeno : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }

        public ECrafteable Tipo => ECrafteable.Oxigeno;

        public Oxigeno() {
            Materiales = new Dictionary<ERecolectable, int>();
            Materiales.Add(ERecolectable.Burbuja, 2);
            Materiales.Add(ERecolectable.Chip, 1);
        }

        public void Craftear(Player player) {
            player.AgregarItem(ECrafteable.Oxigeno);
        }

        public bool PuedeCraftear(Player player) {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
