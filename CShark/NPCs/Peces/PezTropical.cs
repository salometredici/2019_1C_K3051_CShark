using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Peces
{
    public class PezTropical : Pez //esta si requiere especificar el tipo de pez tropical
    {
        public PezTropical(int tipo, float x, float y, float z) 
            : base("Pez Tropical " + tipo, new TGCVector3(x, y, z), 1f, 200f) {

        }

        public override void Aletear(float elapsedTime) {

        }

        public override void Moverse(float elapsedTime) {

        }
    }
}
