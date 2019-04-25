using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Enemigos
{
    public class Rotator
    {
        private float AnguloVisual; //angulo que originalmente esta rotado el mesh respecto a Y
        private int DistanciaHorizontal;
        private int DistanciaVertical;
        private IRotable Objeto;

        public TGCVector3 Punto { get; set; }
        public TGCVector2 Rotacion { get; set; }
        public int Direccion { get; private set; } //de rotacion
        public float Distancia { get; private set; }

        public float CosenoXZ;
        public float SenoXZ;
        public float SenoXY;

        private TGCBox Caja;
        public bool MostrarCaja;

        public Rotator(IRotable objeto, double anguloVisual, int distanciaHorizontal, int distanciaVertical) {
            Objeto = objeto;
            AnguloVisual = (float)anguloVisual;
            DistanciaHorizontal = distanciaHorizontal;
            DistanciaVertical = distanciaVertical;
            Caja = TGCBox.fromSize(TGCVector3.Empty, new TGCVector3(100, 100, 100), Color.Red);
            Caja.AutoTransform = true;
        }

        public void Render() {
            if (MostrarCaja)
                Caja.Render();
        }

        public void GenerarDestino(Mapa mapa) {
            var random = new Random();
            var dirX = random.Next(1, 100) < 50 ? -1 : 1;
            var dirY = random.Next(1, 100) < 50 ? -1 : 1;
            var dirZ = random.Next(1, 100) < 50 ? -1 : 1;
            var distX = random.Next(DistanciaHorizontal, DistanciaHorizontal * 2) * dirX;
            var distY = random.Next(DistanciaVertical, DistanciaVertical * 2) * dirY;
            var distZ = random.Next(DistanciaHorizontal, DistanciaHorizontal * 2) * dirZ;
            Distancia = new TGCVector3(distX, distY, distZ).Length();
            int x = (int)Objeto.Posicion.X + distX;
            int y = (int)Objeto.Posicion.Y + distY;
            int z = (int)Objeto.Posicion.Z + distZ;
            var xReal = x < mapa.Centro.X ? Math.Max(mapa.XMin, x) : Math.Min(mapa.XMax, x);
            var yReal = y < mapa.Centro.Y ? Math.Max(mapa.YMin, y) : Math.Min(mapa.XMax, y);
            var zReal = z < mapa.Centro.Z ? Math.Max(mapa.ZMin, z) : Math.Min(mapa.XMax, z);
            Punto = new TGCVector3(xReal, yReal, zReal);
            Caja.Position = Punto;
        }

        public void GenerarRotacion() {
            var distanciaX = Punto.X - Objeto.Posicion.X;
            var distanciaY = Punto.Y - Objeto.Posicion.Y;
            var distanciaZ = Punto.Z - Objeto.Posicion.Z;
            float anguloHorizontal = (float)Math.Atan2(distanciaZ, distanciaX);
            CosenoXZ = (float)Math.Cos(anguloHorizontal);
            SenoXZ = (float)Math.Sin(anguloHorizontal);
            float diagonalHorizontal = new TGCVector2(distanciaX, distanciaY).Length();
            float anguloVertical = (float)Math.Atan2(distanciaY, diagonalHorizontal);            
            SenoXY = (float)Math.Sin(anguloVertical);
            float anguloVisual = MismoSigno(distanciaX, distanciaZ) ? - AnguloVisual : AnguloVisual;
            var anguloCombinado = anguloHorizontal + anguloVisual;
            float anguloReal = anguloCombinado < 0 ? anguloCombinado + (float)Math.PI * 2 : anguloCombinado;
            float rotacionReal = Objeto.Rotacion.Y < 0 ? Objeto.Rotacion.Y + (float)Math.PI * 2 : Objeto.Rotacion.Y;
            Direccion = anguloReal > rotacionReal ? 1 : -1;
            Rotacion = new TGCVector2(anguloReal, 0);
        }

        private bool MismoSigno(float x, float z) {
            return x > 0 && z > 0 || x < 0 && z < 0;
        }
    }
}
