using CShark.Animales;
using CShark.Animales.Comportamiento;
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
            Comportamiento = new Lineal(1000, 20f, 0.1f);
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
            Escala = 4;
        }
    }
}
