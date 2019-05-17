using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;

namespace CShark.Optimizacion
{
    public class Optimizador
    {
        private Quadtree Quadtree;
        private List<TgcMesh> Meshes;

        public Optimizador() {
            Meshes = new List<TgcMesh>();
            Quadtree = new Quadtree();
        }

        public void CrearQuadtree(TgcBoundingAxisAlignBox bb) {
            Quadtree.create(Meshes, bb);
            Quadtree.createDebugQuadtreeMeshes();
        }

        public void Render(TgcFrustum frustum) {
            Quadtree.render(frustum, true);
        }

        internal void AgregarMesh(TgcMesh mesh) {
            Meshes.Add(mesh);
        }
    }
}
