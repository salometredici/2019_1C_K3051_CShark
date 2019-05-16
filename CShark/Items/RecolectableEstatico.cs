using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Model;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items
{
    public abstract class RecolectableEstatico : Recolectable
    {
        public override TGCVector3 Posicion => Mesh.Position;
        public override TGCVector3 Rotacion => Mesh.Rotation;
        public override TgcBoundingAxisAlignBox Box => _box;

        private TgcMesh Mesh;
        private TgcBoundingAxisAlignBox _box;

        public RecolectableEstatico(TgcMesh mesh) : base(mesh.Position) {
            Mesh = mesh;
            _box = GenerarBox();
            var trasladar = TGCMatrix.Translation(mesh.Position);
            _box.transform(trasladar);
            Mesh.Transform = trasladar;
        }

        public override void Render(GameModel game) {
            if (!Recogido) {
                Mesh.Render();
                _box.Render();
            }
        }

        public override void Dispose() {
            Mesh.Dispose();
        }

        private TgcBoundingAxisAlignBox GenerarBox() {
            var size = Mesh.BoundingBox.calculateSize();
            var lado = size.X > size.Y ? size.X : size.Y;
            lado = size.Z > lado ? size.Z : lado;
            var centro = Mesh.BoundingBox.calculateBoxCenter();
            var m = lado; //escalar si quiero..
            var pmin = new TGCVector3(centro.X - m, centro.Y - m, centro.Z - m);
            var pmax = new TGCVector3(centro.X + m, centro.Y + m, centro.Z + m);
            return new TgcBoundingAxisAlignBox(pmin, pmax);
        }

        public override void Desaparecer() {

        }
    }
}
