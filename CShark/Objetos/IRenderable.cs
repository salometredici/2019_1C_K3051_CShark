using CShark.Model;
using System;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public interface IRenderable : IDisposable
    {
        Material Material { get; }
        TgcMesh Mesh { get; }
        TgcBoundingAxisAlignBox BoundingBox { get; }
        bool Enabled { get; set; }
        void Render(GameModel game);
        void Update(GameModel game);
    }
}
