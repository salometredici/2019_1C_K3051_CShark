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
        public Medkit(TGCVector3 posicion) : base("Medkit", posicion) { }
    }
}
