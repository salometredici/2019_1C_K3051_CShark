using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Animales.Comportamiento
{
    public class Giratorio : IComportamiento
    {
        public float Velocidad;
        private float RadioRotacion;
        private TGCVector3 CentroRotacion;

        public Giratorio(float radio, TGCVector3 centro, float velocidad) {
            Velocidad = velocidad;
            RadioRotacion = radio;
            CentroRotacion = centro;
        }

        private void Moverse(float elapsedTime, Animal animal) {
            var distanciaX = animal.Posicion.X - CentroRotacion.X;
            var distanciaZ = animal.Posicion.Z - CentroRotacion.Z;
            var angulo = Math.Atan2(distanciaZ, distanciaX);
            var d2 = Math.Pow(distanciaX, 2) + Math.Pow(distanciaZ, 2);
            var d = Math.Sqrt(d2);
            float x = (float)(CentroRotacion.X + Math.Cos(angulo + Velocidad * elapsedTime) * d);
            float z = (float)(CentroRotacion.Z + Math.Sin(angulo + Velocidad * elapsedTime) * d);
            animal.Posicion = new TGCVector3(x, 0, z);
            animal.Rotacion += new TGCVector3(0, - Velocidad * elapsedTime, 0);
        }

        public void Render() {
            //nada
        }

        public void Update(float elapsedTime, Animal animal) {
            Moverse(elapsedTime, animal);
        }
    }
}
