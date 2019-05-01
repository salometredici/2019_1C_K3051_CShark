using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Peces
{
    public class PezBetta : Pez
    {
        public PezBetta(float x, float y, float z) 
            : base("Pez Betta", new TGCVector3(x, y, z), 1f, 20f) {

        }

        public PezBetta(TGCVector3 posicion) : this(posicion.X, posicion.Y, posicion.Z) { }


        private float Recorrido = 0;
        private float Aleteado = 0;
        private readonly float DistanciaMaxima = 100f;
        private float AleteoMaximo = 120f / 180 * (float)Math.PI; //120 grados

        public override void Moverse(float elapsedTime) {
            var desplazamiento = VelocidadMovimiento * Direccion * elapsedTime;
            Mesh.Position += new TGCVector3(0, desplazamiento, 0);
            Recorrido += desplazamiento;
            if (FastMath.Abs(Recorrido) > DistanciaMaxima)
            {
                Direccion *= -1;
                Recorrido = 0;
            }
        }

        public override void Aletear(float elapsedTime) {
            var rotar = VelocidadRotacion * DireccionRot * elapsedTime;
            Aleteado += rotar;
            Mesh.Rotation += new TGCVector3(0, rotar, 0); //rota respecto a Y
            if (FastMath.Abs(Aleteado) > AleteoMaximo)
                DireccionRot *= -1;
        }
    }
}
