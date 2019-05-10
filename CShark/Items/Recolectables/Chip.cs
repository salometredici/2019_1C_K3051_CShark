using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items.Recolectables
{
    public class Chip : Recolectable
    {
        public TgcMesh Mesh;
        public override TGCVector3 Posicion => Mesh.Position;
        public override TGCVector3 Rotacion => Mesh.Rotation;
        public override ERecolectable Tipo => ERecolectable.Chip;

        public Chip(TGCVector3 posicion) : base(posicion) {
            Mesh = new TgcSceneLoader()
                .loadSceneFromFile(Game.Default.MediaDirectory + @"Recolectables\Chip-TgcScene.xml")
                .Meshes[0];
            Mesh.AutoTransformEnable = false;
            Mesh.Transform = TGCMatrix.Scaling(2, 2, 2) * TGCMatrix.Translation(posicion);
        }

        public override void Render() {
            if (!Recogido) {
                Mesh.Render();
                EsferaCercania.Render();
            }
        }

        public override void Dispose() {
            Mesh.Dispose();
        }
    }
}
