using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Oxigeno : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Oxigeno;
        public Oxigeno(TGCVector3 posicion) : base("Oxigeno", 4, posicion, 75f, Color.Blue) { }
    }
}
