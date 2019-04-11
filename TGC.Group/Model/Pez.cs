using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Pez
    {
        private float VelocidadRotacion;
        private float VelocidadMovimiento;
        private TgcMesh Mesh;
        private float Direccion = 1f;

        public Pez(TgcMesh mesh) {
            Mesh = mesh;
            VelocidadRotacion = 1f;
            VelocidadMovimiento = 5f;
        }

        public Pez(TgcMesh mesh, float velocidadRotacion, float velocidadMovimiento) {
            Mesh = mesh;
            VelocidadRotacion = velocidadRotacion;
            VelocidadMovimiento = velocidadMovimiento;
        }

        public void Moverse(float elapsed) {
            Mesh.Rotation += new TGCVector3(0, VelocidadRotacion * elapsed, 0);
            Mesh.Position += new TGCVector3(0, VelocidadMovimiento * Direccion * elapsed, 0);

            if (FastMath.Abs(Mesh.Position.Y) > 200f) //si llega a cierta altura
                Direccion *= -1;

            Mesh.Transform = TGCMatrix.RotationYawPitchRoll(Mesh.Rotation.Y, Mesh.Rotation.X, Mesh.Rotation.Z) * TGCMatrix.Translation(Mesh.Position);
        }
    }
}
