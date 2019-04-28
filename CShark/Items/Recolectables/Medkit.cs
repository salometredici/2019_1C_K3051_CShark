using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Medkit : Recolectable
    {
        public Medkit(float x, float y, float z)
            : base("Medkit", new TGCVector3(x, y, z)) { }

    }
}
