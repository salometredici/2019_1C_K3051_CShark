using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.NPCs.Peces
{
    public abstract class Pez
    {
        protected float VelocidadRotacion;
        protected float VelocidadMovimiento;
        protected float DistanciaMaxima;
        protected TgcMesh Mesh;

        protected float Direccion = 1f;
        protected float DireccionRot = 1f;
        protected float Rotado = 0;

        public Pez(TgcMesh mesh, float velocidadRotacion, float velocidadMovimiento, float distanciaMaxima) {
            Mesh = mesh;
            VelocidadRotacion = velocidadRotacion;
            VelocidadMovimiento = velocidadMovimiento;
            DistanciaMaxima = distanciaMaxima;
        }

        public void Update() {
            Moverse();
            Aletear();
        }

        public void Render() {
            Mesh.Render();
        }

        protected abstract void Moverse();
        protected abstract void Aletear();

        protected readonly float RotacionMaxima = 5f / 180 * (float)Math.PI; //5 grados

    }
}
