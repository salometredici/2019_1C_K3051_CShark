using BulletSharp;
using CShark.Fisica;
using CShark.Jugador;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items.Recolectables
{
    public class Wumpa : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Wumpa;
        public Wumpa(TGCVector3 posicion) : base("Wumpa", 2, posicion, 40f) { }
    }
}
