using CShark.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Items.Recolectables
{
    public class Burbuja : Recolectable
    {
        public TGCSphere Esfera;
        public override TGCVector3 Posicion => Esfera.Position;
        public override TGCVector3 Rotacion => Esfera.Rotation;
        public override ERecolectable Tipo => ERecolectable.Burbuja;
        float time = 0;

        public Burbuja(TGCVector3 posicion) : base(posicion) {            
            Esfera = new TGCSphere();
            Esfera.Enabled = true;
            Esfera.Radius = 100f;
            Esfera.Color = Color.Aquamarine;
            Esfera.Position = posicion;            
            Esfera.Effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Burbuja.fx");
            Esfera.Technique = "Burbuja";
            Esfera.AlphaBlendEnable = true;
            Esfera.updateValues();
            Esfera.Transform = TGCMatrix.Scaling(Esfera.Radius, Esfera.Radius, Esfera.Radius) * TGCMatrix.Translation(Esfera.Position);
        }

        public override void Update(GameModel game) {
            base.Update(game);
            time += game.ElapsedTime;
        }

        public override void Render() {            
            if (!Recogido) {
                Esfera.Effect.SetValue("time", time);
                Esfera.Render();
                EsferaCercania.Render();
            }
        }

        public override void Dispose() {
            Esfera.Dispose();
        }
    }
}
