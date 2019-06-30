using CShark.Model;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace CShark.Objetos
{
    public abstract class Brillante : Iluminable
    {
        public TgcTexture Color { get; set; }

        public Brillante(TgcMesh mesh, Material m) : base(mesh, Materiales.Normal) {
            var i = new Random().Next(0, 9);
            Color = TexturasColor.Colores[i];
        }

        public void RenderBrillo() {
            var aux = Mesh.DiffuseMaps.GetValue(0);
            Mesh.DiffuseMaps.SetValue(Color, 0);
            Mesh.Render();
            Mesh.DiffuseMaps.SetValue(aux, 0);
        }
    }
}
