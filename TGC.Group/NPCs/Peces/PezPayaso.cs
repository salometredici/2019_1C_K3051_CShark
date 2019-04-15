using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.NPCs.Peces
{
    public class PezPayaso : Pez
    {
        public PezPayaso(TgcMesh mesh) : base(mesh, 0.001f, 0.25f, 500f) {
            mesh.RotateY(23.3f / 180 * (float) Math.PI);
        }

        private bool _moverse = false;

        protected override void Moverse() {
            if (_moverse)
            {
                var desplazamiento = VelocidadMovimiento * Direccion;
                Mesh.Position += new TGCVector3(0, 0, desplazamiento);
                if (FastMath.Abs(Mesh.Position.Z) > DistanciaMaxima)
                {
                    _moverse = false;
                    Direccion *= -1;
                }
            }
            else
            {
                DarseVuelta();
            }
        }

        protected override void Aletear() {
            if (_moverse)
            {
                var rotar = VelocidadRotacion * DireccionRot;
                Rotado += rotar;
                Mesh.Rotation += new TGCVector3(0, rotar, 0); //rota respecto a Y
                if (FastMath.Abs(Rotado) > RotacionMaxima)
                    DireccionRot *= -1;
            }
        }

        private float _rotado = 0;
        private void DarseVuelta() {
            if (_rotado >= Math.PI)
            {
                _rotado = 0;
                _moverse = true;
            }
            else
            {
                _rotado += VelocidadRotacion * 5; //para que gire un toque mas rapido
                Mesh.Rotation += new TGCVector3(0, VelocidadRotacion * 5, 0);
            }            
        }

    }
}
