using CShark.Jugador;
using System.Collections.Generic;
using System.Linq;

namespace CShark.Items.Crafteables
{
    public class Oro : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }

        public ECrafteable Tipo => ECrafteable.Oro;

        public Oro()
        {
            Materiales = new Dictionary<ERecolectable, int>
            {
                { ERecolectable.Plata, 2 },
                { ERecolectable.Hierro, 2 }
            };
        }

        public void Craftear(Player player)
        {
            player.AgregarItem(ECrafteable.Oro);
        }

        public bool PuedeCraftear(Player player)
        {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
