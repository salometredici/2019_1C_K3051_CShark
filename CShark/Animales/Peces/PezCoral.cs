using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.NPCs.Peces
{
    public class PezCoral : Pez
    {
        public PezCoral(float x, float y, float z) 
            : base("Pez Coral", new TGCVector3(x, y, z), 0.4f, 1000f) {

        }

        public PezCoral(TGCVector3 posicion) : this(posicion.X, posicion.Y, posicion.Z) { }


        public override void Aletear(float elapsedTime) {

        }

        public override void Moverse(float elapsedTime) {

        }
    }
}
