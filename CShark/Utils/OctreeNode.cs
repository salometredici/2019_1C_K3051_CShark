﻿using TGC.Core.SceneLoader;

namespace CShark.Utils
{
    /// <summary>
    ///     Nodo del árbol Octree
    /// </summary>
    internal class OctreeNode
    {
        public OctreeNode[] children;
        public TgcMesh[] models;

        public bool isLeaf()
        {
            return children == null;
        }
    }
}