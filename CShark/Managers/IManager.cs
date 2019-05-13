using CShark.Model;
using System;
using TGC.Core.SceneLoader;

namespace CShark.Managers
{
    public interface IManager : IDisposable
    {
        void Initialize();
        void Render(GameModel game);
        void Update(GameModel game);
    }
}
