using CShark.Terreno;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace CShark.NPCs.Enemigos
{
    public class Tiburon {
        protected float VelocidadRotacion;
        protected float VelocidadMovimiento;
        protected TgcMesh Mesh;
        public TGCVector3 Tamaño { get; set; }
        public TGCVector3 Posicion => Mesh.Position;

        protected float DireccionRot = 1f;
        private float DistanciaMaxima = 500f;
        private float DistanciaMinima = 350f;

        private bool Mover = false;
        private float Recorrido = 0;
        private float Distancia;
        private TGCVector3 PuntoDestino;
        private TGCVector2 RotacionDestino;
        float CosAng;
        float SinAng;
        TGCBox cajaPos = new TGCBox();

        public float Angulo => Mesh.Rotation.Y;

        public Tiburon(float x, float y, float z) {
            var ruta = Game.Default.MediaDirectory + "Animales\\Tiburon-TgcScene.xml";
            Mesh = new TgcSceneLoader().loadSceneFromFile(ruta).Meshes[0];
            Mesh.Position = new TGCVector3(x, y, z);
            VelocidadRotacion = 0.7f;
            VelocidadMovimiento = 300f;
            PuntoDestino = Posicion;
            RotacionDestino = TGCVector2.Zero;
            cajaPos = TGCBox.fromSize(PuntoDestino, new TGCVector3(100, 100, 100), Color.Red);
            cajaPos.AutoTransform = true;
        }

        public void Update(float elapsedTime, Mapa mapa) {
            if (Mover)
                Avanzar(elapsedTime, mapa);
            else
                DarseVuelta(elapsedTime);
        }

        public void Render() {
            Mesh.Render();
            cajaPos.Render();
        }

        private TGCVector3 BuscarPunto(Mapa mapa) {
            var random = new Random();
            var dirX = random.Next(1, 100) < 50 ? -1 : 1;
            var dirY = random.Next(1, 100) < 50 ? -1 : 1;
            var dirZ = random.Next(1, 100) < 50 ? -1 : 1;
            var distX = random.Next((int)DistanciaMinima, (int)DistanciaMaxima) * dirX;
            var distY = random.Next(50, 200) * dirY;
            var distZ = random.Next((int)DistanciaMinima, (int)DistanciaMaxima) * dirZ;
            Distancia = new TGCVector3(distX, distY, distZ).Length();
            int x = (int)Posicion.X + distX;
            int y = (int)Posicion.Y + distY;
            int z = (int)Posicion.Z + distZ;

            var xReal = x < mapa.Centro.X ? Math.Max(mapa.XMin, x) : Math.Min(mapa.XMax, x);
            var yReal = y < mapa.Centro.Y ? Math.Max(mapa.YMin, y) : Math.Min(mapa.XMax, y);
            var zReal = z < mapa.Centro.Z ? Math.Max(mapa.ZMin, z) : Math.Min(mapa.XMax, z);

            return new TGCVector3(xReal, yReal, zReal);
        }

        private TGCVector2 BuscarRotacion() {
            var distanciaX = PuntoDestino.X - Posicion.X;
            var distanciaY = PuntoDestino.Y - Posicion.Y;
            var distanciaZ = PuntoDestino.Z - Posicion.Z;    
            float anguloHorizontal = (float)Math.Atan2(distanciaZ, distanciaX);
            var distHori = new TGCVector2(distanciaX, distanciaY).Length();
            float anguloVertical = (float)Math.Atan2(distanciaY, distHori);
            CosAng = (float)Math.Cos(anguloHorizontal);
            SinAng = (float)Math.Sin(anguloHorizontal);
            var anguloVisual = MismoSigno(distanciaX, distanciaZ)
                ? - Math.PI / 2 : Math.PI / 2;
            return new TGCVector2(anguloHorizontal + (float)anguloVisual, anguloVertical);
        }

        private bool MismoSigno(float x, float z) {
            return x > 0 && z > 0 || x < 0 && z < 0;
        }

        private void Avanzar(float elapsedTime, Mapa mapa) {
            float desplazamientoX = VelocidadMovimiento * elapsedTime * CosAng;
            float desplazamientoY = VelocidadMovimiento * elapsedTime * (float)Math.Sin(RotacionDestino.Y);
            float desplazamientoZ = VelocidadMovimiento * elapsedTime * SinAng;
            var desp = new TGCVector3(desplazamientoX, desplazamientoY, desplazamientoZ);
            Mesh.Position += desp;
            Recorrido += desp.Length();
            if (Recorrido >= Distancia)
            {
                Mover = false;
                Recorrido = 0;
                PuntoDestino = BuscarPunto(mapa);
                RotacionDestino = BuscarRotacion();
                cajaPos.Position = PuntoDestino;
            }
        }

        private void DarseVuelta(float elapsedTime) {
            Mesh.Rotation = new TGCVector3(0, RotacionDestino.X, RotacionDestino.Y);
            Mover = true;
        }
    }
}
