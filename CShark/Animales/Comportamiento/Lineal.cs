using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Animales.Comportamiento
{
    public class Lineal : IComportamiento
    {
        private float VelocidadMovimiento;
        private float VelocidadRotacion;
        private float DistanciaMaxima;
        private bool Mover = false;
        private float Rotado = 0;
        private float Recorrido = 0;
        private float Direccion = 1;

        public Lineal(float distancia, float vMovimiento, float vRotacion) {
            DistanciaMaxima = distancia;
            VelocidadMovimiento = vMovimiento;
            VelocidadRotacion = vRotacion;
        }

        public void Update(float elapsedTime, Animal animal) {
            if (Mover) Avanzar(elapsedTime, animal);
            else DarseVuelta(elapsedTime, animal);
        }

        public void Render() {
            //nada
        }

        private void Avanzar(float elapsedTime, Animal animal) {
            var desplazamiento = VelocidadMovimiento * Direccion * elapsedTime;
            animal.Posicion += new TGCVector3(0, 0, desplazamiento);
            Recorrido += desplazamiento;
            if (FastMath.Abs(Recorrido) > DistanciaMaxima) {
                Mover = false;
                Direccion *= -1;
                Recorrido = 0;
            }
        }

        private void DarseVuelta(float elapsedTime, Animal animal) {
            if (Rotado >= Math.PI) //180°
            {
                Rotado = 0;
                Mover = true;
            }
            else {
                var rotar = VelocidadRotacion * elapsedTime;
                Rotado += rotar;
                animal.Rotacion += new TGCVector3(0, rotar, 0);
            }
        }
    }
}
