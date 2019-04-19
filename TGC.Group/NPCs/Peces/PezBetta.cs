using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.NPCs.Peces
{
    public class PezBetta : Pez
    {
        public PezBetta(float x, float y, float z) 
            : base("Pez Betta", new TGCVector3(x, y, z), 1f, 250f) {

        }

        public override void Aletear(float elapsedTime) {

        }

        public override void Moverse(float elapsedTime) {

        }
    }
}
