using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Animales
{
    public interface IAnimal : IDisposable
    {
        void Update(float elapsedTime);
        void Render();
        void Morir();
        TGCMatrix ArmarTransformacion();
    }
}
