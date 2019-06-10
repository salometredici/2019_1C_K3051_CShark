using CShark.Jugador;
using System.Collections.Generic;
using System.Linq;

namespace CShark.Items.Crafteables
{
    public class AumentoVida : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }

        public ECrafteable Tipo => ECrafteable.AumentoVida;

        public AumentoVida()
        {
            Materiales = new Dictionary<ERecolectable, int>
            {
                { ERecolectable.Oro, 10 },
                { ERecolectable.Coral, 10 }
            };
        }

        public void Craftear(Player player)
        {
            player.AumentarVida();
        }

        public bool PuedeCraftear(Player player)
        {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
