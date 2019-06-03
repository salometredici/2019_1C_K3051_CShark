using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Medkit : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Medkit;
        public Medkit(TGCVector3 posicion) : base("Medkit", 2, posicion, 200f, Color.White) { }
    }
}
