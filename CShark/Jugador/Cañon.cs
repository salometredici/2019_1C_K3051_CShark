using BulletSharp;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.Jugador
{
    public class Cañon : Arma
    {
        private float LargoCañon;
        private TGCSphere Bala;
        private List<RigidBody> Balas;

        public Cañon() : base("Cañon") {
            Bala = new TGCSphere(50f, Color.Black, Posicion);
            Bala.updateValues();
            Balas = new List<RigidBody>();
            LargoCañon = Mesh.BoundingBox.calculateSize().Z;
        }

        public override void Render() {
            base.Render();
            foreach (var bala in Balas)
            {
                Bala.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(bala.InterpolationWorldTransform);
                Bala.Render();
            }
        }

        public override void Atacar(Player player) {
            var camara = player.CamaraInterna;
            var puntaCañon = CalcularPuntaCañon(player);
            var balaBody = BulletRigidBodyFactory.Instance.CreateBall(100f, 1f, puntaCañon);
            var direccionDisparo = new TGCVector3(camara.LookAt.X - camara.Position.X, camara.LookAt.Y - camara.Position.Y, camara.LookAt.Z - camara.Position.Z).ToBulletVector3();
            direccionDisparo.Normalize();
            balaBody.LinearVelocity = direccionDisparo * 900;
            balaBody.LinearFactor = TGCVector3.One.ToBulletVector3();
            Balas.Add(balaBody);
            Mapa.Instancia.AgregarBody(balaBody);
        }

        private TGCVector3 CalcularPuntaCañon(Player player) {
            return TGCVector3.transform(Posicion, Transform);
        }
    }
}
