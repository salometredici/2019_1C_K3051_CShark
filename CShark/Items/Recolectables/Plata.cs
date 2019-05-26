using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Plata : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Plata;
        public Plata(TGCVector3 posicion) : base("Plata", 4, posicion, 120f, Color.Gray) { }
    }
}
