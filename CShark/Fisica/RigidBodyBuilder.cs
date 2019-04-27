using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Fisica
{
    public class RigidBodyBuilder
    {
        private TGCMatrix Transformation;
        private TgcMesh Mesh;

        private float Masa = 1f;
        private float Rebote = 1f;
        private float Damping = 1f;
        private float Rozamiento = 1f;

        public RigidBodyBuilder(TgcMesh mesh) {
            Mesh = mesh;
        }

        public RigidBodyBuilder ConRotacion(TGCVector3 rotacion) {
            float x = rotacion.X, y = rotacion.Y, z = rotacion.Z;
            Transformation = TGCMatrix.Identity * TGCMatrix.RotationYawPitchRoll(y, x, z);
            return this;
        }

        public RigidBodyBuilder ConPosicion(TGCVector3 posicion) {
            Transformation.Origin = posicion;
            return this;
        }

        public RigidBodyBuilder ConMasa(float masa) {
            Masa = masa;
            return this;
        }

        public RigidBodyBuilder ConRozamiento(float rozamiento) {
            Rozamiento = rozamiento;
            return this;
        }

        public RigidBodyBuilder ConDamping(float damping) {
            Damping = damping;
            return this;
        }

        public RigidBodyBuilder ConRebote(float rebote) {
            Rebote = rebote;
            return this;
        }

        public RigidBody Build() {
            var vertexCoords = Mesh.getVertexPositions();
            var triangleMesh = new TriangleMesh();
            for (int i = 0; i < vertexCoords.Length; i = i + 3)
                triangleMesh.AddTriangle(vertexCoords[i].ToBulletVector3(), vertexCoords[i + 1].ToBulletVector3(), vertexCoords[i + 2].ToBulletVector3());
            var motionState = new DefaultMotionState(Transformation.ToBsMatrix);
            var bulletShape = new ConvexTriangleMeshShape(triangleMesh, true);
            var boxLocalInertia = bulletShape.CalculateLocalInertia(Masa);
            var info = new RigidBodyConstructionInfo(Masa, motionState, bulletShape, boxLocalInertia);
            var rigidBody = new RigidBody(info);
            rigidBody.Friction = Rozamiento;
            rigidBody.Restitution = Rebote;
            rigidBody.SetDamping(0.5f, 0.1f);
            return rigidBody;
        }
    }
}
