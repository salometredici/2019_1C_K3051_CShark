using BulletSharp;
using BulletSharp.Math;
using CShark.Animales.Enemigos;
using CShark.Fisica;
using CShark.Fisica.Colisiones;
using CShark.Geometria;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace CShark.NPCs.Enemigos
{
    public class Tiburon : IRotable {
        protected float VelocidadRotacion;
        protected float VelocidadMovimiento;
        protected TgcMesh Mesh;
        public TGCVector3 Posicion => Mesh.Position;
        public TGCVector3 Rotacion => Mesh.Rotation;
        
        private bool Mover = false;
        private float Recorrido = 0;
        private bool Vivo = true;
        private float Vida;
        public RigidBody Body;
        private Rotator Rotador;

        private HealthBar BarraVida;

        private float TiempoInvencibilidad; //segundos
        
        public Tiburon(float x, float y, float z) {
            var ruta = Game.Default.MediaDirectory + "Animales\\Tiburon-TgcScene.xml";
            Mesh = new TgcSceneLoader().loadSceneFromFile(ruta).Meshes[0];
            Mesh.Position = new TGCVector3(x, y, z);
            VelocidadRotacion = 1f;
            VelocidadMovimiento = 300f;
            Rotador = new Rotator(this, Math.PI / 2, 300, 100);
            Rotador.MostrarCaja = true;
            Rotador.GenerarDestino();
            Rotador.GenerarRotacion();
            Vida = 300f;
            TiempoInvencibilidad = 1f;
            BarraVida = new HealthBar(Vida);
            var builder = new RigidBodyBuilder("Tiburon");
            Body = builder.ConDamping(1f)
                .ConRotacion(Rotacion).ConPosicion(Posicion)
                .ConRebote(10f).ConRozamiento(1f)
                .ConMasa(0f)//para que sea estatico
                .Build();
            Mapa.Instancia.AgregarBody(Body);
        }

        public void Update(float elapsedTime) {
            TiempoInvencibilidad -= elapsedTime;
            if (Vivo)
            {
                Body.WorldTransform = TGCMatrix.Translation(Posicion).ToBsMatrix;
                BarraVida.Update(Vida, Posicion, Rotacion);
                if (Mover)
                    Avanzar(elapsedTime);
                else
                    DarseVuelta(elapsedTime);
            }
        }

        public void RecibirDaño(float daño) {
            if (TiempoInvencibilidad <= 0) {
                Vida -= daño;
                TiempoInvencibilidad = 1f;
                if (Vida <= 0)
                    Morir();
            }
        }

        public void Render() {
            if (!Vivo)
            {
                //gracias al auto enable en morir, pero probablemente cambiar :)
                Mesh.Transform = new TGCMatrix(Body.InterpolationWorldTransform);
            }
            Mesh.Render();
            Rotador.Render();
            if (Vivo) 
                BarraVida.Render(Vida);
        }

        public void Morir() {
            Vivo = false;
            Mesh.AutoTransformEnable = false; //ESTO ME VA A ARRUINAR LA VIDA EN EL FUTURO PROBABLEMENTE
            Mapa.Instancia.SacarBody(Body);
            Body.SetMassProps(500f, Vector3.Zero);
            var x = VelocidadMovimiento * Rotador.CosenoXZ;
            var z = VelocidadMovimiento * Rotador.SenoXZ;
            Body.LinearVelocity = new Vector3(x, 0, z);
            Mapa.Instancia.AgregarBody(Body);
        }

        private void Avanzar(float elapsedTime) {
            float desplazamientoX = VelocidadMovimiento * elapsedTime * Rotador.CosenoXZ;
            float desplazamientoY = VelocidadMovimiento * elapsedTime * Rotador.SenoXY;
            float desplazamientoZ = VelocidadMovimiento * elapsedTime * Rotador.SenoXZ;
            var desp = new TGCVector3(desplazamientoX, desplazamientoY, desplazamientoZ);
            Mesh.Position += desp;
            Recorrido += desp.Length();
            if (Recorrido >= Rotador.Distancia)
            {
                Mover = false;
                Recorrido = 0;
                Rotador.GenerarDestino();
                Rotador.GenerarRotacion();
            }
        }

        private void DarseVuelta(float elapsedTime) {
            bool condicion = Rotador.Direccion == 1
                ? Rotacion.Y >= Rotador.Rotacion.X
                : Rotacion.Y <= Rotador.Rotacion.X;

            if (condicion)
            {
                Mover = true;
            }
            else
            {
                var rotar = VelocidadRotacion * elapsedTime * Rotador.Direccion;
                Mesh.Rotation += new TGCVector3(0, rotar, 0);
                LimitarAngulo();
            }
        }

        //si la rotacion se pasa de 360 me aseguro que vuelva a contar desde 0°, para que el float 
        //no se vaya a la mierda y me arruine las cuentas :)
        private void LimitarAngulo() {
            if (FastMath.Abs(Rotacion.Y) > Math.PI * 2)
            {
                var vueltas = FastMath.Abs(Rotacion.Y) / (Math.PI * 2);
                var angulo = vueltas - Math.Truncate(vueltas);
                Mesh.Rotation = new TGCVector3(0, (float)angulo, 0);
            }
        }
    }
}
