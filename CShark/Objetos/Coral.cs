using CShark.EfectosLuces;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Coral : Iluminable
    {
        public Coral(TgcMesh mesh) : base(mesh, Materiales.Normal) { }
    }
}
