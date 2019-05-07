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
    public class PezCoral : Animal
    {
        public PezCoral(TGCVector3 posicion) : base("Pez Betta", posicion) {
            Comportamiento = new Giratorio(100f, posicion, 150f);
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
        }
    }
}
