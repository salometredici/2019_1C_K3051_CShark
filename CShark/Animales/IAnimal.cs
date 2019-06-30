using CShark.Model;
using CShark.Objetos;
using System;
using TGC.Core.Mathematica;

namespace CShark.Animales
{
    public interface IAnimal : IRenderable
    {
        void Update(GameModel game);
        void Render(GameModel game);
        void Morir();
        TGCMatrix ArmarTransformacion();
    }
}
