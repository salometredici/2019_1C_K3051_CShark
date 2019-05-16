using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.Luces
{
    public class Luz : IDisposable
    {
        public Color Color;
        public TGCVector3 Posicion;
        public float Intensidad;
        public float Atenuacion;
        public float Especular;
        private TGCBox Cubito;

        public Luz(Color color, TGCVector3 posicion, float intensidad, float atenuacion, float especular) {
            Cubito = TGCBox.fromSize(posicion, new TGCVector3(20, 20, 20), color);
            Color = color;
            Posicion = posicion;
            Intensidad = intensidad;
            Atenuacion = atenuacion;
            Especular = especular;
        }

        public void Update(TGCVector3 posicion) {
            Posicion = posicion;
            Cubito.Transform = TGCMatrix.Translation(posicion);
        }

        public void Render() {
            //Cubito.Render();
        }

        public void Dispose() {
            Cubito.Dispose();
        }
    }
}
