using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Interpolation;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace CShark.Items.Recolectables
{
    public class Chip : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Chip;
        public Chip(TGCVector3 posicion) : base("Chip", 2, posicion, 100f, Color.Green) { }
    }
}
