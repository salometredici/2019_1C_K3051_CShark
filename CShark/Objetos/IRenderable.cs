using System;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public interface IRenderable : IDisposable
    {
        Material Material { get; }
        TgcMesh Mesh { get; }
        void Render(TGCVector3 camara);
    }
}
