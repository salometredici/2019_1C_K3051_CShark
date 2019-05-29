using CShark.Objetos;
using System.Collections.Generic;
using System.Linq;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Optimizaciones
{
    public class Octree
    {
        private readonly OctreeBuilder builder;
        private List<IRenderable> Objetos;
        private OctreeNode octreeRootNode;
        private TgcBoundingAxisAlignBox sceneBounds;

        public Octree() {
            builder = new OctreeBuilder();
        }

        public void create(List<IRenderable> objetos, TgcBoundingAxisAlignBox sceneBounds) {
            Objetos = objetos;
            this.sceneBounds = sceneBounds;

            octreeRootNode = builder.crearOctree(objetos, sceneBounds);

            foreach (var objeto in objetos)
                objeto.Enabled = false;
        }

        public void render(TgcFrustum frustum) {
            var pMax = sceneBounds.PMax;
            var pMin = sceneBounds.PMin;
            findVisibleMeshes(frustum, octreeRootNode,
                pMin.X, pMin.Y, pMin.Z,
                pMax.X, pMax.Y, pMax.Z);
            foreach (var mesh in Objetos) {
                if (mesh.Enabled) {
                    mesh.Render();
                    mesh.Enabled = false;
                }
            }
        }

        private void findVisibleMeshes(TgcFrustum frustum, OctreeNode node,
            float boxLowerX, float boxLowerY, float boxLowerZ,
            float boxUpperX, float boxUpperY, float boxUpperZ) {
            var children = node.children;

            if (children == null) {
                selectLeafMeshes(node);
            }

            else {
                var midX = FastMath.Abs((boxUpperX - boxLowerX) / 2);
                var midY = FastMath.Abs((boxUpperY - boxLowerY) / 2);
                var midZ = FastMath.Abs((boxUpperZ - boxLowerZ) / 2);
                testChildVisibility(frustum, children[0], boxLowerX + midX, boxLowerY + midY, boxLowerZ + midZ,
                    boxUpperX, boxUpperY, boxUpperZ);
                testChildVisibility(frustum, children[1], boxLowerX + midX, boxLowerY + midY, boxLowerZ, boxUpperX,
                    boxUpperY, boxUpperZ - midZ);
                testChildVisibility(frustum, children[2], boxLowerX + midX, boxLowerY, boxLowerZ + midZ, boxUpperX,
                    boxUpperY - midY, boxUpperZ);
                testChildVisibility(frustum, children[3], boxLowerX + midX, boxLowerY, boxLowerZ, boxUpperX,
                    boxUpperY - midY, boxUpperZ - midZ);
                testChildVisibility(frustum, children[4], boxLowerX, boxLowerY + midY, boxLowerZ + midZ,
                    boxUpperX - midX, boxUpperY, boxUpperZ);
                testChildVisibility(frustum, children[5], boxLowerX, boxLowerY + midY, boxLowerZ, boxUpperX - midX,
                    boxUpperY, boxUpperZ - midZ);
                testChildVisibility(frustum, children[6], boxLowerX, boxLowerY, boxLowerZ + midZ, boxUpperX - midX,
                    boxUpperY - midY, boxUpperZ);
                testChildVisibility(frustum, children[7], boxLowerX, boxLowerY, boxLowerZ, boxUpperX - midX,
                    boxUpperY - midY, boxUpperZ - midZ);
            }
        }

        private void testChildVisibility(TgcFrustum frustum, OctreeNode childNode,
            float boxLowerX, float boxLowerY, float boxLowerZ, float boxUpperX, float boxUpperY, float boxUpperZ) {
            if (childNode == null)
                return;
            var caja = new TgcBoundingAxisAlignBox(
                new TGCVector3(boxLowerX, boxLowerY, boxLowerZ),
                new TGCVector3(boxUpperX, boxUpperY, boxUpperZ));
            var c = TgcCollisionUtils.classifyFrustumAABB(frustum, caja);
            if (c == TgcCollisionUtils.FrustumResult.INSIDE) 
                addAllLeafMeshes(childNode);
            else if (c == TgcCollisionUtils.FrustumResult.INTERSECT)
                findVisibleMeshes(frustum, childNode, boxLowerX, boxLowerY, boxLowerZ, boxUpperX, boxUpperY, boxUpperZ);
        }

        private void addAllLeafMeshes(OctreeNode node) {
            if (node == null)
                return;

            var children = node.children;
            if (children == null)
                selectLeafMeshes(node);
            else {
                for (var i = 0; i < children.Length; i++)
                    addAllLeafMeshes(children[i]);
            }
        }

        private void selectLeafMeshes(OctreeNode node) {
            var models = node.models;
            foreach (var m in models)
                m.Enabled = true;
        }
    }
}