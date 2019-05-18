using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Luces;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Roca : IRenderable
    {
        public TgcMesh Mesh { get; }
        public Material Material { get; }

        public Roca(TgcMesh mesh) {
            Mesh = mesh;
            Mesh.Effect = Iluminacion.EfectoLuz;
            Mesh.Technique = "Iluminado";
            Material = Materiales.Roca;
        }

        public void Render(TGCVector3 camara) {
            Iluminacion.ActualizarEfecto(Mesh.Effect, Material, camara);
            Mesh.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
        }
    }
}
