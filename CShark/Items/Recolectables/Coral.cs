using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items.Recolectables
{
    public class Coral : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Coral;
        public Coral(TGCVector3 posicion) : base("Coral", 2, posicion, 75f, Color.Pink) { }
    }
}
