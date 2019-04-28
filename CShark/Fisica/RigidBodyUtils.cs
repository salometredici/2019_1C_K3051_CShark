using BulletSharp;
using BulletSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.SceneLoader;

namespace CShark.Fisica
{
    public static class RigidBodyUtils
    {
        public static float CalcularRadio(TgcMesh mesh) {
            return mesh.BoundingBox.calculateBoxRadius();
        }

        public static RigidBody CrearEsfera(TgcMesh mesh, float masa) {
            var radio = CalcularRadio(mesh);
            return BulletRigidBodyFactory.Instance.CreateBall(radio, masa, mesh.Position);
        }
    }
}
