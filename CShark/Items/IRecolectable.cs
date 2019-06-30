using CShark.Jugador;
using CShark.Model;
using CShark.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;

namespace CShark.Items
{
    public interface IRecolectable : IRenderable
    {
        TgcBoundingSphere EsferaCercania { get; }
        ERecolectable Tipo { get; }
        bool PuedeRecoger(Player player);
    }

    public enum ERecolectable
    {
        Pila,
        Chip,
        Burbuja,
        Wumpa,
        Coral,
        Pez,
        Oro,
        Plata, 
        Hierro,
        Medkit,
        Oxigeno,
        Arpon
    }
}
