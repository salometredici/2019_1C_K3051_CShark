using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Optimizacion
{
    public class Quadtree
    {
        private readonly QuadtreeBuilder Builder;
        private List<TgcBoxDebug> DebugQuadtreeBoxes;
        private List<TgcMesh> Meshes;
        private QuadtreeNode QuadtreeRootNode;
        private TgcBoundingAxisAlignBox Bounds;

        public Quadtree() {
            Builder = new QuadtreeBuilder();
        }

        public void create(List<TgcMesh> meshes, TgcBoundingAxisAlignBox bounds) {
            Meshes = meshes;
            Bounds = bounds;
            QuadtreeRootNode = Builder.crearQuadtree(Meshes, Bounds);
            meshes.ForEach(m => m.Enabled = false);
        }

        public void createDebugQuadtreeMeshes() {
            DebugQuadtreeBoxes = Builder.createDebugQuadtreeMeshes(QuadtreeRootNode, Bounds);
        }

        public void render(TgcFrustum frustum, bool debugEnabled) {
            var pMax = Bounds.PMax;
            var pMin = Bounds.PMin;

            findVisibleMeshes(frustum, QuadtreeRootNode,
                pMin.X, pMin.Y, pMin.Z,
                pMax.X, pMax.Y, pMax.Z);

            foreach (var mesh in Meshes) {
                if (mesh.Enabled) {
                    mesh.Render();
                    mesh.Enabled = false;
                }
            }

            if (debugEnabled) {
                foreach (var debugBox in DebugQuadtreeBoxes) {
                    debugBox.Render();
                }
            }
        }

        private void findVisibleMeshes(TgcFrustum frustum, QuadtreeNode node,
            float boxLowerX, float boxLowerY, float boxLowerZ,
            float boxUpperX, float boxUpperY, float boxUpperZ) {
            var children = node.children;

            //es hoja, cargar todos los meshes
            if (children == null) {
                selectLeafMeshes(node);
            }

            //recursividad sobre hijos
            else {
                var midX = FastMath.Abs((boxUpperX - boxLowerX) / 2);
                var midZ = FastMath.Abs((boxUpperZ - boxLowerZ) / 2);

                //00
                testChildVisibility(frustum, children[0], boxLowerX + midX, boxLowerY, boxLowerZ + midZ, boxUpperX,
                    boxUpperY, boxUpperZ);

                //01
                testChildVisibility(frustum, children[1], boxLowerX + midX, boxLowerY, boxLowerZ, boxUpperX, boxUpperY,
                    boxUpperZ - midZ);

                //10
                testChildVisibility(frustum, children[2], boxLowerX, boxLowerY, boxLowerZ + midZ, boxUpperX - midX,
                    boxUpperY, boxUpperZ);

                //11
                testChildVisibility(frustum, children[3], boxLowerX, boxLowerY, boxLowerZ, boxUpperX - midX, boxUpperY,
                    boxUpperZ - midZ);
            }
        }

        private void testChildVisibility(TgcFrustum frustum, QuadtreeNode childNode,
            float boxLowerX, float boxLowerY, float boxLowerZ, float boxUpperX, float boxUpperY, float boxUpperZ) {
            //test frustum-box intersection
            var caja = new TgcBoundingAxisAlignBox(
                new TGCVector3(boxLowerX, boxLowerY, boxLowerZ),
                new TGCVector3(boxUpperX, boxUpperY, boxUpperZ));
            var c = TgcCollisionUtils.classifyFrustumAABB(frustum, caja);

            //complementamente adentro: cargar todos los hijos directamente, sin testeos
            if (c == TgcCollisionUtils.FrustumResult.INSIDE) {
                addAllLeafMeshes(childNode);
            }

            //parte adentro: seguir haciendo testeos con hijos
            else if (c == TgcCollisionUtils.FrustumResult.INTERSECT) {
                findVisibleMeshes(frustum, childNode, boxLowerX, boxLowerY, boxLowerZ, boxUpperX, boxUpperY, boxUpperZ);
            }
        }

        /// <summary>
        ///     Hacer visibles todas las meshes de un nodo, buscando recursivamente sus hojas
        /// </summary>
        private void addAllLeafMeshes(QuadtreeNode node) {
            var children = node.children;

            //es hoja, cargar todos los meshes
            if (children == null) {
                selectLeafMeshes(node);
            }
            //pedir hojas a hijos
            else {
                for (var i = 0; i < children.Length; i++) {
                    addAllLeafMeshes(children[i]);
                }
            }
        }

        /// <summary>
        ///     Hacer visibles todas las meshes de un nodo
        /// </summary>
        private void selectLeafMeshes(QuadtreeNode node) {
            var models = node.models;
            foreach (var m in models) {
                m.Enabled = true;
            }
        }
    }
}