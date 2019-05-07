using CShark.Animales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Peces
{
    public class PezTropical : Animal
    {
        public PezTropical(int tipo, TGCVector3 posicion) : base("Pez Tropical " + tipo, posicion) {
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
        }
    }
}
