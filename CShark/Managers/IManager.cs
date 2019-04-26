using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Managers
{
    public interface IManager
    {
        void Initialize();
        void Render(GameModel game);
        void Update(GameModel game);
    }
}
