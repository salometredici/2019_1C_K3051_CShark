using CShark.Model;
using System;
using TGC.Core.Mathematica;

namespace CShark.Animales
{
    public interface IAnimal : IDisposable
    {
        void Update(GameModel game);
        void Render(GameModel game);
        void Morir();
        TGCMatrix ArmarTransformacion();
    }
}
