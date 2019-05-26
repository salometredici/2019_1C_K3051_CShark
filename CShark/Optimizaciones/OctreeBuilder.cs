using CShark.Objetos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Optimizaciones
{
    internal class OctreeBuilder
    {
        private readonly int MAX_SECTOR_OCTREE_RECURSION = 3;
        private readonly int MIN_MESH_PER_LEAVE_THRESHOLD = 5;

        public OctreeNode crearOctree(List<IRenderable> objetos, TgcBoundingAxisAlignBox sceneBounds) {
            var rootNode = new OctreeNode();
            var pMax = sceneBounds.PMax;
            var pMin = sceneBounds.PMin;
            var midSize = sceneBounds.calculateAxisRadius();
            var center = sceneBounds.calculateBoxCenter();
            doSectorOctreeX(rootNode, center, midSize, 0, objetos);
            deleteEmptyNodes(rootNode.children);
            return rootNode;
        }

        private void doSectorOctreeX(OctreeNode parent, TGCVector3 center, TGCVector3 size,
            int step, List<IRenderable> meshes) {
            var x = center.X;
            var possitiveList = new List<IRenderable>();
            var negativeList = new List<IRenderable>();
            var xCutPlane = new TGCPlane(1, 0, 0, -x);
            splitByPlane(xCutPlane, meshes, possitiveList, negativeList);
            doSectorOctreeY(parent, new TGCVector3(x + size.X / 2, center.Y, center.Z),
                new TGCVector3(size.X / 2, size.Y, size.Z),
                step, possitiveList, 0);
            doSectorOctreeY(parent, new TGCVector3(x - size.X / 2, center.Y, center.Z),
                new TGCVector3(size.X / 2, size.Y, size.Z),
                step, negativeList, 4);
        }

        private void doSectorOctreeY(OctreeNode parent, TGCVector3 center, TGCVector3 size, int step,
            List<IRenderable> meshes, int childIndex) {
            var y = center.Y;
            var possitiveList = new List<IRenderable>();
            var negativeList = new List<IRenderable>();
            var yCutPlane = new TGCPlane(0, 1, 0, -y);
            splitByPlane(yCutPlane, meshes, possitiveList, negativeList);
            doSectorOctreeZ(parent, new TGCVector3(center.X, y + size.Y / 2, center.Z),
                new TGCVector3(size.X, size.Y / 2, size.Z),
                step, possitiveList, childIndex + 0);
            doSectorOctreeZ(parent, new TGCVector3(center.X, y - size.Y / 2, center.Z),
                new TGCVector3(size.X, size.Y / 2, size.Z),
                step, negativeList, childIndex + 2);
        }

        private void doSectorOctreeZ(OctreeNode parent, TGCVector3 center, TGCVector3 size, int step,
            List<IRenderable> meshes, int childIndex) {
            var z = center.Z;
            var possitiveList = new List<IRenderable>();
            var negativeList = new List<IRenderable>();
            var zCutPlane = new TGCPlane(0, 0, 1, -z);
            splitByPlane(zCutPlane, meshes, possitiveList, negativeList);
            if (parent.children == null)
                parent.children = new OctreeNode[8];
            var posNode = new OctreeNode();
            parent.children[childIndex] = posNode;
            var negNode = new OctreeNode();
            parent.children[childIndex + 1] = negNode;
            if (step >= MAX_SECTOR_OCTREE_RECURSION || meshes.Count <= MIN_MESH_PER_LEAVE_THRESHOLD) {
                posNode.models = possitiveList.ToArray();
                negNode.models = negativeList.ToArray();
            }
            else {
                step++;
                doSectorOctreeX(posNode, new TGCVector3(center.X, center.Y, z + size.Z / 2),
                    new TGCVector3(size.X, size.Y, size.Z / 2),
                    step, possitiveList);
                doSectorOctreeX(negNode, new TGCVector3(center.X, center.Y, z - size.Z / 2),
                    new TGCVector3(size.X, size.Y, size.Z / 2),
                    step, negativeList);
            }
        }

        private void splitByPlane(TGCPlane cutPlane, List<IRenderable> modelos,
            List<IRenderable> possitiveList, List<IRenderable> negativeList) {
            TgcCollisionUtils.PlaneBoxResult c;
            foreach (var modelo in modelos) {
                c = TgcCollisionUtils.classifyPlaneAABB(cutPlane, modelo.BoundingBox);
                if (c == TgcCollisionUtils.PlaneBoxResult.IN_FRONT_OF)
                    possitiveList.Add(modelo);
                else if (c == TgcCollisionUtils.PlaneBoxResult.BEHIND)
                    negativeList.Add(modelo);
                else {
                    possitiveList.Add(modelo);
                    negativeList.Add(modelo);
                }
            }
        }

        private void deleteEmptyNodes(OctreeNode[] children) {
            if (children == null)
                return;
            for (var i = 0; i < children.Length; i++) {
                var childNode = children[i];
                var childNodeChildren = childNode.children;
                if (childNodeChildren != null && hasEmptyChilds(childNode)) {
                    childNode.children = null;
                    childNode.models = new IRenderable[0];
                }
                else
                    deleteEmptyNodes(childNodeChildren);
            }
        }

        private bool hasEmptyChilds(OctreeNode node) {
            var children = node.children;
            for (var i = 0; i < children.Length; i++) {
                var childNode = children[i];
                if (childNode.children != null || childNode.models.Length > 0)
                    return false;
            }
            return true;
        }

        private void deleteSameMeshCountChilds(OctreeNode node) {
            if (node == null || node.children == null) {
                return;
            }

            var nodeCount = getTotalNodeMeshCount(node);
            for (var i = 0; i < node.children.Length; i++) {
                var childNode = node.children[i];
                var childCount = getTotalNodeMeshCount(childNode);
                if (childCount == nodeCount) {
                    node.children[i] = null;
                }
                else {
                    deleteSameMeshCountChilds(node.children[i]);
                }
            }
        }

        private int getTotalNodeMeshCount(OctreeNode node) {
            if (node.children == null)
                return node.models.Length;
            var meshCount = 0;
            for (var i = 0; i < node.children.Length; i++)
                meshCount += getTotalNodeMeshCount(node.children[i]);
            return meshCount;
        }
    }
}