using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Animales
{
    public interface IComportamiento
    {
        void Update(float elapsedTime, Animal animal);
        void Render();
    }
}
