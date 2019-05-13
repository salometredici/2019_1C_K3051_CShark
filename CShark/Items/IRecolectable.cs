using CShark.Jugador;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;

namespace CShark.Items
{
    public interface IRecolectable : IDisposable
    {
        TgcBoundingSphere EsferaCercania { get; }
        TgcBoundingAxisAlignBox Box { get; }
        ERecolectable Tipo { get; }
        void Update(GameModel game);
        void Render(GameModel game);
        bool PuedeRecoger(Player player);
    }

    public enum ERecolectable
    {
        Bateria,
        Burbuja,
        Chip,
        Wumpa
    }
}
