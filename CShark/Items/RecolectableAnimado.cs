using CShark.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Interpolation;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace CShark.Items
{
    public abstract class RecolectableAnimado : Recolectable
    {
        public TgcMesh Mesh;
        private float rotacion;
        private TGCVector3 posicion;
        private TGCVector3 escala;
        private float offsetLetra;
        private InterpoladorVaiven Interpolador;
        private TgcMesh LetraE1;
        private TgcMesh LetraE2;

        public override TGCVector3 Posicion => Mesh.Position;
        public override TGCVector3 Rotacion => Mesh.Rotation;

        public RecolectableAnimado(string mesh, float _escala, TGCVector3 _posicion, float _offsetLetra) : base(_posicion) {
            var loader = new TgcSceneLoader();
            Mesh = loader.loadSceneFromFile(Game.Default.MediaDirectory + @"Recolectables\" + mesh + "-TgcScene.xml").Meshes[0];
            LetraE1 = loader.loadSceneFromFile(Game.Default.MediaDirectory + @"Recolectables\LetraE-TgcScene.xml").Meshes[0];
            LetraE2 = LetraE1.clone("E2");
            LetraE1.AutoTransformEnable = false;
            Mesh.AutoTransformEnable = false;
            LetraE2.AutoTransformEnable = false;
            escala = new TGCVector3(_escala, _escala, _escala);
            rotacion = 0f;
            posicion = _posicion;
            offsetLetra = _offsetLetra;
            Interpolador = new InterpoladorVaiven {
                Min = -30f,
                Max = 30f,
                Current = 0,
                Speed = 135f
            };
        }

        private TGCMatrix GetLetraTransform(float offsetX) {
            var yRot = TGCMatrix.RotationY(FastMath.PI / 2);
            var offset = TGCMatrix.Translation(new TGCVector3(offsetX, 0, 0));
            var orbita = TGCMatrix.RotationY(rotacion);
            var reposicion = TGCMatrix.Translation(posicion);
            return yRot * offset * orbita * reposicion;
        }

        private TGCMatrix GetMeshTransform(float elapsedTime) {
            var yRot = TGCMatrix.RotationY(rotacion);
            var scale = TGCMatrix.Scaling(escala);
            var pos = TGCMatrix.Translation(posicion + new TGCVector3(0, Interpolador.update(elapsedTime), 0));
            return yRot * scale * pos;
        }

        public override void Render(GameModel game) {
            if (!Recogido) {
                rotacion += game.ElapsedTime * 2f;
                Mesh.Transform = GetMeshTransform(game.ElapsedTime);
                LetraE1.Transform = GetLetraTransform(offsetLetra);
                LetraE2.Transform = GetLetraTransform(-offsetLetra);
                Mesh.Render();
                if (PuedeRecoger(game.Player)) {
                    LetraE1.Render();
                    LetraE2.Render();
                }
            }
        }

        public override void Dispose() {
            Mesh.Dispose();
        }
    }
}
