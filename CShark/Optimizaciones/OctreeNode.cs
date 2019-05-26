using CShark.Objetos;
using TGC.Core.SceneLoader;

namespace CShark.Optimizaciones
{
    internal class OctreeNode
    {
        public OctreeNode[] children;
        public IRenderable[] models;

        public bool isLeaf() {
            return children == null;
        }
    }
}