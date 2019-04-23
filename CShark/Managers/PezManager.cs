using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using CShark.NPCs.Peces;

namespace CShark.Managers
{
    public class PezManager
    {
        private List<Pez> Peces;

        public PezManager() {
            Peces = new List<Pez>();
        }

        public void Update(float elapsedTime) {
            Peces.ForEach(pez => pez.Update(elapsedTime));
        }

        public void Render() {
            Peces.ForEach(pez => pez.Render());
        }

        public void CargarPez(Pez pez) {
            Peces.Add(pez);
        }
    }
}
