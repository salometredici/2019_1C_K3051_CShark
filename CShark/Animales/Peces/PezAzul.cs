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
    public class PezAzul : Animal
    {
        public PezAzul(TGCVector3 posicion) : base("Pez Azul", posicion) {
            Comportamiento = new Giratorio(300f, posicion, 250f);
            Rotacion = new TGCVector3(0, -(float)Math.PI / 2, 0);
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
        }
    }
}
