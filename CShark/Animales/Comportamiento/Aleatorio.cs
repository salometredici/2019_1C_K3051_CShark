using CShark.Geometria;
using System;
using TGC.Core.Mathematica;

namespace CShark.Animales.Comportamiento
{
    public class Aleatorio : IComportamiento
    {
        public float VelocidadRotacion;
        public float VelocidadMovimiento;
        private bool Mover = false;
        private float Recorrido = 0;
        private Rotator Rotador;

        public Aleatorio(double rotacionInicialMesh, int maximoXZ, int maximoY, float vMovimiento, float vRotacion) {
            Rotador = new Rotator(rotacionInicialMesh, maximoXZ, maximoY);
            Rotador.MostrarCaja = false;
            VelocidadMovimiento = vMovimiento;
            VelocidadRotacion = vRotacion;
        }

        public void Update(float elapsedTime, Animal animal) {
            if (Mover) Avanzar(elapsedTime, animal);
            else DarseVuelta(elapsedTime, animal);
        }

        public void Render() {
            Rotador.Render();
        }

        private void Avanzar(float elapsedTime, Animal animal) {
            float desplazamientoX = VelocidadMovimiento * elapsedTime * Rotador.CosenoXZ;
            float desplazamientoY = VelocidadMovimiento * elapsedTime * Rotador.SenoXY;
            float desplazamientoZ = VelocidadMovimiento * elapsedTime * Rotador.SenoXZ;
            var desp = new TGCVector3(desplazamientoX, desplazamientoY, desplazamientoZ);
            animal.Posicion += desp;
            Recorrido += desp.Length();
            if (Recorrido >= Rotador.Distancia) {
                Mover = false;
                Recorrido = 0;
                Rotador.GenerarDestino(animal);
                Rotador.GenerarRotacion(animal);
            }
        }

        private void DarseVuelta(float elapsedTime, Animal animal) {
            bool condicion = Rotador.Direccion == 1
                ? animal.Rotacion.Y >= Rotador.Rotacion.X
                : animal.Rotacion.Y <= Rotador.Rotacion.X;

            if (condicion)
                Mover = true;
            else
                Rotar(animal, VelocidadRotacion * elapsedTime * Rotador.Direccion);
        }

        private void Rotar(Animal animal, float grados) {
            animal.Rotacion += new TGCVector3(0, grados, 0);
            LimitarAngulo(animal);
        }

        public void LimitarAngulo(Animal animal) {
            if (FastMath.Abs(animal.Rotacion.Y) > Math.PI * 2) {
                var vueltas = FastMath.Abs(animal.Rotacion.Y) / (Math.PI * 2);
                var angulo = vueltas - Math.Truncate(vueltas);
                animal.Rotacion = new TGCVector3(0, (float)angulo, 0);
            }
        }
    }
}
