using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.EfectosLuces;
using CShark.Model;
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
            Material = Materiales.Roca;
        }

        public void Render() {
            Mesh.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Update(GameModel game) {
            Efectos.ActualizarLuces(Mesh.Effect, Material, game.Player.Posicion);
        }
    }
}
