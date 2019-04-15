using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.NPCs.Peces
{
    public class PezCoral : Pez
    {
        public PezCoral(TgcMesh mesh) : base(mesh, 0.1f, 0.4f, 1000f) { }

        protected override void Aletear() {
            throw new NotImplementedException();
        }

        protected override void Moverse() {
            throw new NotImplementedException();
        }
    }
}
