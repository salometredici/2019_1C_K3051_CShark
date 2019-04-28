using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Bateria : Recolectable
    {
        public Bateria(float x, float y, float z)
            : base("Bateria", new TGCVector3(x, y, z)) { }

    }
}
