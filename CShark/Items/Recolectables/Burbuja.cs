using CShark.Model;
using CShark.Objetos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace CShark.Items.Recolectables
{
    public class Burbuja : Recolectable
    {
        private TGCSphere Esfera;
        public override TgcBoundingAxisAlignBox BoundingBox => _box; //lo calculo una sola vez..
        public override TGCVector3 Posicion => Esfera.Position;
        public override TGCVector3 Rotacion => Esfera.Rotation;
        public override ERecolectable Tipo => ERecolectable.Burbuja;
        float time = 0;
        private TgcMesh _mesh;

        public override TgcMesh Mesh => _mesh;

        public override Material Material => Materiales.Normal;

        private TgcBoundingAxisAlignBox _box;

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
            _box = CalcularBox();
            _mesh = Esfera.toMesh("b");
        }

        private TgcBoundingAxisAlignBox CalcularBox() {
            var radio = Esfera.BoundingSphere.Radius;
            var p = Esfera.BoundingSphere.Position;
            var distanciaAlCentro = new TGCVector3(radio, radio, radio);
            var pmin = p - distanciaAlCentro;
            var pmax = p + distanciaAlCentro;
            return new TgcBoundingAxisAlignBox(pmin, pmax);
        }

        public override void Update(GameModel game) {
            base.Update(game);
            time += game.ElapsedTime;
        }

        public override void Render(GameModel game) {
            if (!Recogido) {
                Esfera.Effect.SetValue("time", time);
                Esfera.Render();
            }
        }

        public override void Dispose() {
            Esfera.Dispose();
        }

        public override void Desaparecer() {

        }
    }
}
