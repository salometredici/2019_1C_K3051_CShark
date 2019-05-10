using CShark.Jugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Items
{
    public interface ICrafteable
    {
        ECrafteable Tipo { get; }
        Dictionary<ERecolectable, int> Materiales { get; }        
        void Craftear(Player player);
        bool PuedeCraftear(Player player);
    }

    public enum ECrafteable
    {
        Arpon,
        Oxigeno,
        Medkit
    }
}
