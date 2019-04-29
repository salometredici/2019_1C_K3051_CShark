using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.Animales.Enemigos
{
    public class HealthBar
    {
        public TGCVector3 Posicion;
        public TGCVector3 Rotacion;
        private TGCVector3 Escala;
        private float ValorMaximo;
        private float ValorAnterior = 0;

        private TGCBox Borde;
        private TGCBox Rectangulo;

        private readonly int AnchoBarra = 300;
        private readonly int AltoBarra = 40;
        private TGCVector3 OffsetAltura;

        public HealthBar(float valorMaximo) {
            ValorMaximo = valorMaximo;
            Borde = TGCBox.fromSize(new TGCVector3(AnchoBarra, AltoBarra, 1f), Color.Black);
            Rectangulo = TGCBox.fromSize(new TGCVector3(AnchoBarra - 10, AltoBarra - 10, 3f), Color.LimeGreen);
            OffsetAltura = new TGCVector3(0, 200f, 0);
            Escala = new TGCVector3(1f, 1f, 1f);
        }

        public void Update(float valor, TGCVector3 posicion, TGCVector3 rotacion) {
            Posicion = posicion + OffsetAltura;
            Rotacion = rotacion;
            if (valor != ValorAnterior)
                Escala = new TGCVector3(valor / ValorMaximo, 1, 1);
            ValorAnterior = valor;
            var transformacion = TGCMatrix.Scaling(Escala) * TGCMatrix.RotationY(Rotacion.Y) * TGCMatrix.Translation(Posicion);
            Borde.Transform = transformacion;
            Rectangulo.Transform = transformacion;
        }

        public void Render(float valor) {
            Borde.Render();
            Rectangulo.Render();
        }
    }
}
