using CShark.Luces;
using CShark.Model;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Items.Recolectables
{
    public class Bateria : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Bateria;

        public Bateria(TGCVector3 posicion) : base("Bateria", 4, posicion, 75f, Color.Yellow) { }

    }
}
