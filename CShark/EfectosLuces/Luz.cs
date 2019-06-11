using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.EfectosLuces
{
    public class Luz
    {
        public Color Color;
        public TGCVector3 Posicion;
        public float Intensidad;
        public float Atenuacion;

        public Luz(Color color, TGCVector3 posicion, float intensidad, float atenuacion) {
            Color = color;
            Posicion = posicion;
            Intensidad = intensidad;
            Atenuacion = atenuacion;
        }

        public float Distancia(TGCVector3 punto) {
            var x = Posicion.X - punto.X;
            var y = Posicion.Y - punto.Y;
            var z = Posicion.Z - punto.Z;
            return FastMath.Sqrt(x * x + y * y + z * z);
        }
    }
}
