using CShark.EfectosLuces;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Palmera : IRenderable
    {
        public TgcMesh Mesh { get; }
        public Material Material { get; }
        public TgcBoundingAxisAlignBox BoundingBox => Mesh.BoundingBox;
        public bool Enabled {
            get => Mesh.Enabled;
            set => Mesh.Enabled = value;
        }

        public Palmera(TgcMesh mesh) {
            Mesh = mesh;
            Material = Materiales.Madera;
        }

        public void Render() {
            Mesh.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Update(GameModel game) {
            //Efectos.Instancia.ActualizarLuces(Mesh.Effect, Material, game.Player.Posicion);
        }
    }
}
