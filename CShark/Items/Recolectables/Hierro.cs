using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Hierro : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Hierro;
        public Hierro(TGCVector3 posicion) : base("Hierro", 2, posicion, 120f, Color.Brown) { }
    }
}
