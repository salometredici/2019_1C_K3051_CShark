using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Enemigos
{
    public interface IRotable
    {
        TGCVector3 Posicion { get; }
        TGCVector3 Rotacion { get; }
    }
}
