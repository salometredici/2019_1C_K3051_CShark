using BulletSharp;
using CShark.Fisica;
using CShark.Jugador;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Items.Recolectables
{
    public class Wumpa : Recolectable
    {
        RigidBody Body;

        public Wumpa(float x, float y, float z) : base("Wumpa", new TGCVector3(x, y, z)) {
            Body = RigidBodyUtils.CrearEsfera(Mesh, 10f);
            Body.Friction = 1f;
            Body.RollingFriction = 100f;
            Mapa.Instancia.AgregarBody(Body);
        }

        public override void Update(Player player) {
            var x = Body.Orientation.Axis.X;
            var y = Body.Orientation.Axis.Y;
            var z = Body.Orientation.Axis.Z;
            Rotacion = new TGCVector3(x, y, z);
            var centro = Body.CenterOfMassPosition;
            Posicion = new TGCVector3(centro.X, centro.Y, centro.Z);
            base.Update(player);
        }
    }
}
