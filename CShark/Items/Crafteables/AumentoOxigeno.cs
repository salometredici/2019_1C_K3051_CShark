using CShark.Jugador;
using System.Collections.Generic;
using System.Linq;

namespace CShark.Items.Crafteables
{
    public class AumentoOxigeno : ICrafteable
    {
        public Dictionary<ERecolectable, int> Materiales { get; }

        public ECrafteable Tipo => ECrafteable.AumentoOxigeno;

        public AumentoOxigeno()
        {
            Materiales = new Dictionary<ERecolectable, int>
            {
                { ERecolectable.Wumpa, 10 },
                { ERecolectable.Burbuja, 5 }
            };
        }

        public void Craftear(Player player)
        {
            player.AgregarItem(ECrafteable.AumentoOxigeno); // Esto no va a ser así
        }

        public bool PuedeCraftear(Player player)
        {
            return Materiales.All(m =>
                player.CuantosTiene(m.Key) >= m.Value
            );
        }
    }
}
