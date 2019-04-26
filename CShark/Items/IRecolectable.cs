using CShark.Jugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Items
{
    public interface IRecolectable
    {
        void Desaparecer();
        void Render();
        bool EstaCerca(Player player);
    }
}
