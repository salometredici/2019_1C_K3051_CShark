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
        private float DireccionX = 1f;
        private float DireccionY = 1f;
        
        public Pez(TgcMesh mesh, float velocidadRotacion, float velocidadMovimiento) {
            Mesh = mesh;
            VelocidadRotacion = velocidadRotacion;
            VelocidadMovimiento = velocidadMovimiento;
        }

        public void Moverse(float elapsed) {
            //Mesh.Rotation += new TGCVector3(0, VelocidadRotacion * elapsed, 0);
            var despX = VelocidadMovimiento * DireccionX * 0.025f; //num magico
            var despY = VelocidadMovimiento * DireccionY * 0.025f;

            Mesh.Position += new TGCVector3(despX, despY, 0);

            if (FastMath.Abs(Mesh.Position.X) > 100f)
                DireccionX *= -1;
            if (FastMath.Abs(Mesh.Position.Y) > 400f) //si llega a cierta altura
                DireccionY *= -1;

            Mesh.Transform = TGCMatrix.RotationYawPitchRoll(Mesh.Rotation.Y, Mesh.Rotation.X, Mesh.Rotation.Z) * TGCMatrix.Translation(Mesh.Position);
        }
    }
}
