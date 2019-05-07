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
        public Chip(TGCVector3 posicion) : base("Chip", posicion) { }
    }
}
