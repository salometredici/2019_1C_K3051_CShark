using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Arpon : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Arpon;
        public Arpon(TGCVector3 posicion) : base("Arpon", 4, posicion, 50f, Color.Red) { }
    }
}
