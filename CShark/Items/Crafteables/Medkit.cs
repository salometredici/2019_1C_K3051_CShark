using CShark.Jugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Items.Crafteables
{
    public class Medkit : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }

        public ECrafteable Tipo => ECrafteable.Medkit;

        public Medkit() {
            Materiales = new Dictionary<ERecolectable, int>();
            Materiales.Add(ERecolectable.Bateria, 2);
        }

        public void Craftear(Player player) {
            player.AgregarItem(ECrafteable.Medkit);
        }

        public bool PuedeCraftear(Player player) {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
