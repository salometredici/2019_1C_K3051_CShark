using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.NPCs.Peces
{
    public class PezAzul : Pez
    {
        private float RadioRotacion;
        private TGCVector3 CentroRotacion;
        private float Rotado = 0;

        public PezAzul(float x, float y, float z) 
            : base("Pez Azul", new TGCVector3(x, y, z), 1f, 250f) {
            Mesh.Rotation = new TGCVector3(0, -(float)Math.PI / 2, 0); //-90 grados, arreglo villero por ahora
            RadioRotacion = 300f;
            CentroRotacion = new TGCVector3(Mesh.Position.X + RadioRotacion, Mesh.Position.Y, Mesh.Position.Z);
        }

        public override void Moverse(float elapsedTime) {
            var distanciaX = Mesh.Position.X - CentroRotacion.X;
            var distanciaZ = Mesh.Position.Z - CentroRotacion.Z;
            var angulo = Math.Atan2(distanciaZ, distanciaX);
            var d2 = Math.Pow(distanciaX, 2) + Math.Pow(distanciaZ, 2);
            var d = Math.Sqrt(d2);
            float x = (float)(CentroRotacion.X + Math.Cos(angulo + VelocidadRotacion * elapsedTime) * d);
            float z = (float)(CentroRotacion.Z + Math.Sin(angulo + VelocidadRotacion * elapsedTime) * d);
            Mesh.Position = new TGCVector3(x, 0, z);
            Mesh.Rotation += new TGCVector3(0, - VelocidadRotacion * elapsedTime , 0);
        }

        public override void Aletear(float elapsedTime) {
            /*var rotar = VelocidadRotacion * DireccionRot * elapsedTime;
            Rotado += rotar;
            Mesh.Rotation += new TGCVector3(0, rotar, 0); //rota respecto a Y
            if (FastMath.Abs(Rotado) > RotacionMaxima)
                DireccionRot *= -1;*/
        }
    }
}
