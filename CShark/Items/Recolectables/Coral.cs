using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace CShark.Items.Recolectables
{
    public class Coral : RecolectableEstatico
    {
        public override ERecolectable Tipo => ERecolectable.Coral;
        public Coral(TgcMesh mesh) : base(mesh) { }
    }
}
