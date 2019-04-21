using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.NPCs.Peces
{
    public class PezPayaso : Pez
    {
        public PezPayaso(float x, float y, float z) 
            : base("Pez Payaso", new TGCVector3(x, y, z), 1f, 250f) { }

        private bool Mover = false;
        private float Rotado = 0;
        private float Aleteado = 0;
        private float Recorrido = 0;
        private readonly float DistanciaMaxima = 500f;
        protected float AleteoMaximo = 5f / 180 * (float)Math.PI; //5 grados

        public override void Moverse(float elapsedTime) {
            if (Mover)
                Avanzar(elapsedTime);
            else
                DarseVuelta(elapsedTime);
        }

        public override void Aletear(float elapsedTime) {
            if (Mover)
            {
                var rotar = VelocidadRotacion * DireccionRot * elapsedTime;
                Aleteado += rotar;
                Mesh.Rotation += new TGCVector3(0, rotar, 0); //rota respecto a Y
                if (FastMath.Abs(Aleteado) > AleteoMaximo)
                    DireccionRot *= -1;
            }
        }

        private void Avanzar(float elapsedTime) {
            var desplazamiento = VelocidadMovimiento * Direccion * elapsedTime;
            Mesh.Position += new TGCVector3(0, 0, desplazamiento);
            Recorrido += desplazamiento;
            if (FastMath.Abs(Recorrido) > DistanciaMaxima)
            {
                Mover = false;
                Direccion *= -1;
                Recorrido = 0;
            }
        }
        
        private void DarseVuelta(float elapsedTime) {
            if (Rotado >= Math.PI) //180°
            {
                Rotado = 0;
                Mover = true;
            }
            else
            {
                var rotar = VelocidadRotacion * elapsedTime;
                Rotado += rotar;
                Mesh.Rotation += new TGCVector3(0, rotar, 0);
            }            
        }

    }
}
