using CShark.EfectosLuces;
using CShark.Model;
using CShark.Objetos;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Direct3D;
using TGC.Core.Interpolation;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace CShark.Terreno
{
    public class Sol : Brillante
    {
        public TGCVector3 Posicion;
        private float Rotacion;
        public bool AlphaBlendEnable { get; set; }
        public Luz Luz;

        public Sol(TgcMesh mesh, TGCVector3 posicion) : base(mesh, Materiales.Normal) {
            Posicion = posicion;
            Rotacion = 0;
            Luz = new Luz(System.Drawing.Color.White, Posicion, 3000f, 0.1f);
            Mesh.AutoTransformEnable = false;
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\SunTexture.jpg";
            var tex = TgcTexture.createTexture(D3DDevice.Instance.Device, path);
            Mesh.changeDiffuseMaps(new[] {
                tex
            });
            Efectos.Instancia.AgregarLuz(Luz);
        }

        public void Update(float elapsedTime) {
            Rotacion += elapsedTime * 0.5f;
        }

        private TGCMatrix GetTransform() {
            var scale = TGCMatrix.Scaling(100, 100, 100);
            var position = TGCMatrix.Translation(Posicion);
            var rotation = TGCMatrix.RotationY(Rotacion);
            return scale * rotation * position;
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Render() {
            Mesh.Transform = this.GetTransform();
            Mesh.Render();
        }
    }
}
