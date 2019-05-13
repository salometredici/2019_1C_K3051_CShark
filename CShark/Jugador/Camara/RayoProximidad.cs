using BulletSharp;
using CShark.Managers;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;

namespace CShark.Jugador.Camara
{
    public class RayoProximidad
    {
        private TgcArrow Flecha;
        private TgcRay Rayo;
        private TGCVector3 Offset;
        private bool MostrarFlecha => Configuracion.Instancia.MostrarRayo.Valor;

        public RayoProximidad() {
            Rayo = new TgcRay();
            Offset = new TGCVector3(15, 0, 0);
            Flecha = new TgcArrow {
                BodyColor = Color.Red,
                HeadColor = Color.Green,
                Thickness = 2,
                HeadSize = new TGCVector2(10, 10)
            };
        }

        public void Update(TgcFpsCamera camara, TGCVector3 posicion) {
            var anguloLateral = MathUtil.NormalizeAngle(camara.leftrightRot - FastMath.PI);
            var anguloVertical = MathUtil.NormalizeAngle(camara.updownRot);
            var distanciaPosta = 2000f;
            var distanciaXZ = FastMath.Cos(anguloVertical) * distanciaPosta;
            var x = FastMath.Sin(anguloLateral) * distanciaXZ;
            var z = FastMath.Cos(anguloLateral) * distanciaXZ;
            var y = FastMath.Sin(anguloVertical) * distanciaPosta;
            var origen = posicion + Offset;
            var destino = origen + new TGCVector3(x, y, z);
            Rayo.Origin = origen;
            Rayo.Direction = destino - origen;
            if (MostrarFlecha) {
                Flecha.PStart = origen;
                Flecha.PEnd = destino;
                Flecha.updateValues();
            }
        }

        public void Render() {
            if (MostrarFlecha) Flecha.Render();
        }

        public bool Intersecta(TgcBoundingAxisAlignBox box) {
            var punto = new TGCVector3();
            return TgcCollisionUtils.intersectRayAABB(Rayo, box, out punto);
        }
    }
}
