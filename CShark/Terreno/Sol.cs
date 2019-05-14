using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace CShark.Terreno
{
    public class Sol : IRenderObject
    {
        private TgcMesh Mesh;
        public TGCVector3 Posicion;
        private float Rotacion;
        public bool AlphaBlendEnable { get; set; }

        public float Intensidad => 4000f;
        public Color ColorLuz; 

        public Sol(TGCVector3 posicion) {
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\SunTexture.jpg";
            var tex = TgcTexture.createTexture(D3DDevice.Instance.Device, path);
            var sphere = Game.Default.MediaDirectory + @"Mapa\Sphere-TgcScene.xml";
            Posicion = posicion;
            ColorLuz = Color.White;
            Rotacion = 0;
            Mesh = new TgcSceneLoader().loadSceneFromFile(sphere).Meshes[0];
            Mesh.AutoTransformEnable = false;
            Mesh.changeDiffuseMaps(new[] {
                tex
            });
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Update(float elapsedTime) {
            Rotacion += elapsedTime * 0.5f;
        }

        public void Render() {
            Mesh.Transform = GetTransform();
            Mesh.Render();
        }

        private TGCMatrix GetTransform() {
            var scale = TGCMatrix.Scaling(100, 100, 100);
            var position = TGCMatrix.Translation(Posicion);
            var rotation = TGCMatrix.RotationY(Rotacion);
            return scale * rotation * position;
        }
    }
}
