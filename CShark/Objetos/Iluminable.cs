using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.EfectosLuces;
using CShark.Model;
using Microsoft.DirectX.Direct3D;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public abstract class Iluminable : IRenderable
    {
        public TgcMesh Mesh { get; set; }
        public Material Material { get; set; }
        public Effect Efecto { get; set; }
        public string Tecnica { get; set; }

        public Iluminable(Material material) {
            Efecto = Efectos.Instancia.EfectoLuzNiebla;
            Material = material;
        }

        public Iluminable(TgcMesh mesh, Material material) : this(material) {
            Tecnica = "NubladoIluminado";
            Mesh = mesh;
        }

        public TgcBoundingAxisAlignBox BoundingBox => Mesh.BoundingBox;

        public bool Enabled {
            get => Mesh.Enabled;
            set => Mesh.Enabled = value;
        }

        private void ActualizarLuces(TGCVector3 camara) {
            var contenedor = ContenedorLuces.Instancia;

            Efecto.SetValue("coloresLuces", contenedor.Colores);
            Efecto.SetValue("posicionesLuces", contenedor.Posiciones);
            Efecto.SetValue("intensidadesLuces", contenedor.Intensidades);
            Efecto.SetValue("atenuacionesLuces", contenedor.Atenuaciones);
            Efecto.SetValue("cantidadLuces", contenedor.Cantidad);

            Efecto.SetValue("colorEmisivo", Material.Emisivo);
            Efecto.SetValue("colorDifuso", Material.Difuso);
            Efecto.SetValue("colorEspecular", Material.Especular);
            Efecto.SetValue("exponenteEspecular", Material.Brillito);
            Efecto.SetValue("colorAmbiente", Material.Ambiente);

            Efecto.SetValue("posicionCamara", TGCVector3.Vector3ToFloat4Array(camara));
        }

        public virtual void Dispose() {
            if (Mesh != null)
                Mesh.Dispose();
        }

        public virtual void Render(GameModel game) {
            this.ActualizarLuces(game.Camara.Position);
            if (Mesh != null)
                Mesh.Render();
        }

        public virtual void RenderOscuro() {
            if (Mesh != null) {
                Mesh.Technique = "DibujarObjetosOscuros";
                Mesh.Render();
                Mesh.Technique = "NubladoIluminado";
            }
        }

        public virtual void Update(GameModel game) {
            //Basura
        }
    }
}
