using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items.Recolectables
{
    public class Bateria : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Bateria;
        public Bateria(TGCVector3 posicion) : base("Bateria", 4, posicion, 75f) { }
    }
}
