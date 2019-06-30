using CShark.Animales;
using CShark.Animales.Comportamiento;
using System;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Peces
{
    public class PezBetta : Animal
    {
        public PezBetta(TGCVector3 posicion) : base("Pez Betta", posicion) {
            Comportamiento = new Giratorio(300f, posicion, 200f);
            Body = BulletRigidBodyFactory.Instance.CreateBall(10f, 50f, posicion);
            Escala = 6;
        }
    }
}
