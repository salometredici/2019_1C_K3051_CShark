using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.EfectosLuces;
using CShark.Model;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Roca : Iluminable
    {
        public Roca(TgcMesh mesh) : base(mesh, Materiales.Roca) { }
    }
}
