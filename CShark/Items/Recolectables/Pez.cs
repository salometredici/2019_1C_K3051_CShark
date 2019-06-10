using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Pez : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Pez;
        public Pez(TGCVector3 posicion) : base("Pez", 4, posicion, 100f, Color.OrangeRed) { }
    }
}
