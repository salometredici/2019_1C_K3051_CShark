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

namespace TGC.Group.NPCs.Enemigos
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
        private float RotacionDestino;
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
            PuntoDestino = BuscarPunto();
            RotacionDestino = BuscarRotacion();
            cajaPos = TGCBox.fromSize(PuntoDestino, new TGCVector3(100, 100, 100), Color.Red);
            cajaPos.AutoTransform = true;
        }

        public void Update(float elapsedTime) {
            if (Mover)
                Avanzar(elapsedTime);
            else
                DarseVuelta(elapsedTime);
        }

        public void Render() {
            Mesh.Render();
            cajaPos.Render();
        }

        private TGCVector3 BuscarPunto() {
            var random = new Random();
            var dirX = random.Next(1, 100) < 50 ? -1 : 1; //sidoso
            var dirZ = random.Next(1, 100) < 50 ? -1 : 1;
            var distX = random.Next((int)DistanciaMinima, (int)DistanciaMaxima) * dirX;
            var distZ = random.Next((int)DistanciaMinima, (int)DistanciaMaxima) * dirZ;
            Distancia = (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distZ, 2));
            int x = (int)Posicion.X + distX;
            int z = (int)Posicion.Z + distZ;
            int y = (int)Posicion.Y;
            return new TGCVector3(x, y, z);
        }

        private float BuscarRotacion() {
            var distanciaX = PuntoDestino.X - Posicion.X;
            var distanciaZ = PuntoDestino.Z - Posicion.Z;
            float anguloHorizontal = (float)Math.Atan2(distanciaZ, distanciaX);
            CosAng = (float)Math.Cos(anguloHorizontal);
            SinAng = (float)Math.Sin(anguloHorizontal);
            var anguloVisual = MismoSigno(distanciaX, distanciaZ)
                ? - Math.PI / 2 : Math.PI / 2;
            return anguloHorizontal + (float)anguloVisual;
        }

        private bool MismoSigno(float x, float z) {
            return x > 0 && z > 0 || x < 0 && z < 0;
        }

        private void Avanzar(float elapsedTime) {
            float desplazamientoX = VelocidadMovimiento * elapsedTime * CosAng;
            float desplazamientoZ = VelocidadMovimiento * elapsedTime * SinAng;
            var distancia = (float)Math.Sqrt(Math.Pow(desplazamientoX, 2) + Math.Pow(desplazamientoZ, 2));
            Mesh.Position += new TGCVector3(desplazamientoX, 0, desplazamientoZ);
            Recorrido += distancia;
            if (Recorrido >= Distancia)
            {
                Mover = false;
                Recorrido = 0;
                PuntoDestino = BuscarPunto();
                RotacionDestino = BuscarRotacion();
                cajaPos.Position = PuntoDestino;
            }
        }

        private void DarseVuelta(float elapsedTime) {
            Mesh.Rotation = new TGCVector3(0, RotacionDestino, 0);
            Mover = true;
        }
    }
}
