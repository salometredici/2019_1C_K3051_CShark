using CShark.Model;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Items.Recolectables
{
    public class Pila : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Pila;

        public Pila(TGCVector3 posicion) : base("Pila", 4, posicion, 75f, Color.Yellow) { }

    }
}
