using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Interpolation;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Items
{
    public class LuzItem
    {
        private Effect Effect;
        private InterpoladorVaiven Interpolador;
        private TGCBox Cajita;
        private TGCVector3 Posicion;

        //datos para el technique
        public Vector4 PosicionLuz;
        public float IntensidadLuz;
        public float AtenuacionLuz;
        public ColorValue ColorLuz;

        public LuzItem(Color color, TGCVector3 posicion) {
            Effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "LuzItem.fx");
            Cajita = TGCBox.fromSize(new TGCVector3(10, 10, 10), color);
            Posicion = posicion;
            Interpolador = new InterpoladorVaiven {
                Min = -100f,
                Max = 100f,
                Speed = 100f,
                Current = 0f
            };
            PosicionLuz = TGCVector3.Vector3ToVector4(Posicion);
            IntensidadLuz = 1000f;
            AtenuacionLuz = 0.1f;
            ColorLuz = ColorValue.FromColor(color);
        }

        public void Update(float elapsedTime) {
            var move = new TGCVector3(0, 0, Interpolador.update(elapsedTime));
            Cajita.Position = Posicion + TGCVector3.Scale(move, 1);
            Cajita.Transform = TGCMatrix.Translation(Cajita.Position);
            PosicionLuz = TGCVector3.Vector3ToVector4(Posicion);
        }

        public void Render() {
            Cajita.Render();
        }
    }
}
