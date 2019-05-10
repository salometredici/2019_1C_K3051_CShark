using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Jugador;

namespace CShark.Items.Crafteables
{
    public class Arpon : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }
        
        public ECrafteable Tipo => ECrafteable.Arpon;

        public Arpon() {
            Materiales = new Dictionary<ERecolectable, int>();
            Materiales.Add(ERecolectable.Bateria, 2);
        }

        public void Craftear(Player player) {
            player.AgregarItem(ECrafteable.Arpon);
        }

        public bool PuedeCraftear(Player player) {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
