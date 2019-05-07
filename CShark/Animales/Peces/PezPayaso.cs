using CShark.Animales;
using CShark.Animales.Comportamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.NPCs.Peces
{
    public class PezPayaso : Animal
    {
        public PezPayaso(TGCVector3 posicion) : base("Pez Payaso", posicion) {
            Comportamiento = new Lineal(500f, 250f, 1f);
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
        }
    }
}
