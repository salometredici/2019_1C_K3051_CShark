﻿using CShark.Animales;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.Geometria
{
    public class Rotator
    {
        private float AnguloVisual; //angulo que originalmente esta rotado el mesh respecto a Y
        private int DistanciaHorizontal;
        private int DistanciaVertical;

        public TGCVector3 Punto { get; set; }
        public TGCVector2 Rotacion { get; set; }
        public int Direccion { get; private set; } //de rotacion
        public float Distancia { get; private set; }

        public float CosenoXZ;
        public float SenoXZ;
        public float SenoXY;

        private TGCBox Caja;
        public bool MostrarCaja;

        public Rotator(double anguloVisual, int distanciaHorizontal, int distanciaVertical) {
            AnguloVisual = (float)anguloVisual;
            DistanciaHorizontal = distanciaHorizontal;
            DistanciaVertical = distanciaVertical;
            Caja = TGCBox.fromSize(TGCVector3.Empty, new TGCVector3(100, 100, 100), Color.Red);
            Caja.AutoTransformEnable = true;
        }

        public void Render() {
            if (MostrarCaja)
                Caja.Render();
        }

        public void GenerarDestino(Animal animal) {
            var mapa = Mapa.Instancia;
            var random = new Random();
            var dirX = random.Next(1, 100) < 50 ? -1 : 1;
            var dirY = random.Next(1, 100) < 50 ? -1 : 1;
            var dirZ = random.Next(1, 100) < 50 ? -1 : 1;
            var distX = random.Next(DistanciaHorizontal, DistanciaHorizontal * 2) * dirX;
            var distY = random.Next(DistanciaVertical, DistanciaVertical * 2) * dirY;
            var distZ = random.Next(DistanciaHorizontal, DistanciaHorizontal * 2) * dirZ;
            Distancia = new TGCVector3(distX, distY, distZ).Length();
            int x = (int)animal.Posicion.X + distX;
            int y = (int)animal.Posicion.Y + distY;
            int z = (int)animal.Posicion.Z + distZ;
            var xReal = x < mapa.Centro.X ? Math.Max(mapa.XMin, x) : Math.Min(mapa.XMax, x);
            var yReal = y < mapa.Centro.Y ? Math.Max(mapa.YMin, y) : Math.Min(mapa.XMax, y);
            var zReal = z < mapa.Centro.Z ? Math.Max(mapa.ZMin, z) : Math.Min(mapa.XMax, z);
            Punto = new TGCVector3(xReal, yReal, zReal);
            Caja.Position = Punto;
        }

        public void GenerarRotacion(Animal animal) {
            var distanciaX = Punto.X - animal.Posicion.X;
            var distanciaY = Punto.Y - animal.Posicion.Y;
            var distanciaZ = Punto.Z - animal.Posicion.Z;
            float anguloHorizontal = (float)Math.Atan2(distanciaZ, distanciaX);
            CosenoXZ = (float)Math.Cos(anguloHorizontal);
            SenoXZ = (float)Math.Sin(anguloHorizontal);
            float diagonalHorizontal = new TGCVector2(distanciaX, distanciaY).Length();
            float anguloVertical = (float)Math.Atan2(distanciaY, diagonalHorizontal);            
            SenoXY = (float)Math.Sin(anguloVertical);
            float anguloVisual = MismoSigno(distanciaX, distanciaZ) ? - AnguloVisual : AnguloVisual;
            var anguloCombinado = anguloHorizontal + anguloVisual;
            float anguloReal = anguloCombinado < 0 ? anguloCombinado + (float)Math.PI * 2 : anguloCombinado;
            float rotacionReal = animal.Rotacion.Y < 0 ? animal.Rotacion.Y + (float)Math.PI * 2 : animal.Rotacion.Y;
            Direccion = anguloReal > rotacionReal ? 1 : -1;
            Rotacion = new TGCVector2(anguloReal, 0);
        }

        private bool MismoSigno(float x, float z) {
            return x > 0 && z > 0 || x < 0 && z < 0;
        }
    }
}
