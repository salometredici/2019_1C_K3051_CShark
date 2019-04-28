using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Chip : Recolectable
    {
        public Chip(float x, float y, float z)
            : base("Chip", new TGCVector3(x, y, z)) { }

    }
}
