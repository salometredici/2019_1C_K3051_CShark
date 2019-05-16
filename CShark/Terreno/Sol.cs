﻿using CShark.Luces;
using System.Drawing;
using TGC.Core.Direct3D;
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

        public Luz Luz;

        public Sol(TGCVector3 posicion) {
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\SunTexture.jpg";
            var tex = TgcTexture.createTexture(D3DDevice.Instance.Device, path);
            var sphere = Game.Default.MediaDirectory + @"Mapa\Sphere-TgcScene.xml";
            Posicion = posicion;
            Rotacion = 0;
            Luz = new Luz(Color.White, Posicion, 2000f, 0.1f, 20f);
            Iluminacion.AgregarLuz(Luz);
            Mesh = new TgcSceneLoader().loadSceneFromFile(sphere).Meshes[0];
            Mesh.AutoTransformEnable = false;
            Mesh.changeDiffuseMaps(new[] {
                tex
            });
        }

        public void Dispose() {
            Luz.Dispose();
            Mesh.Dispose();
        }

        public void Update(float elapsedTime) {
            Luz.Update(Posicion);
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
