using CShark.EfectosLuces;
using CShark.Model;
using CShark.Objetos;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using TGC.Core.Textures;
using static Microsoft.DirectX.Direct3D.CustomVertex;
using Material = CShark.Objetos.Material;

namespace CShark.Terreno
{
    public class Suelo : IDisposable
    {
        public TgcMesh Terreno;
        public bool AlphaBlendEnable => Terreno.AlphaBlendEnable;
        public TGCVector3 Center => TGCVector3.Empty;
        private Material Material;
        private Texture TexturaRayoSol;
        float time = 0;

        public Suelo() {
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno2.png";
            Terreno = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Terreno-TgcScene.xml").Meshes[0];
            Terreno.Effect = Efectos.Instancia.EfectoLuzNiebla;
            Terreno.Technique = "SueloNubladoIluminado";
            Material = Materiales.Arena;
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\fondo.png";
            TexturaRayoSol = TgcTexture.createTexture(D3DDevice.Instance.Device, path).D3dTexture;
        }

        public void Update(float elapsedTime) {
            time += elapsedTime;
        }

        public void Render(GameModel game) {
            Terreno.Effect.SetValue("texRayosSol", TexturaRayoSol);
            Terreno.Effect.SetValue("time", time);
            this.ActualizarLuces(game.Camara.Position);
            Terreno.Render();
            //this.DibujarWireFrame();
        }

        private void DibujarWireFrame() {
            D3DDevice.Instance.Device.RenderState.FillMode = FillMode.WireFrame;
            D3DDevice.Instance.Device.RenderState.Lighting = false;
            Terreno.D3dMesh.DrawSubset(0);
            D3DDevice.Instance.Device.RenderState.FillMode = FillMode.Solid;
            D3DDevice.Instance.Device.RenderState.Lighting = true;
        }

        private void ActualizarLuces(TGCVector3 camara) {
            var contenedor = ContenedorLuces.Instancia;
            Terreno.Effect.SetValue("coloresLuces", contenedor.Colores);
            Terreno.Effect.SetValue("posicionesLuces", contenedor.Posiciones);
            Terreno.Effect.SetValue("intensidadesLuces", contenedor.Intensidades);
            Terreno.Effect.SetValue("atenuacionesLuces", contenedor.Atenuaciones);
            Terreno.Effect.SetValue("cantidadLuces", contenedor.Cantidad);
            Terreno.Effect.SetValue("colorEmisivo", Material.Emisivo);
            Terreno.Effect.SetValue("colorDifuso", Material.Difuso);
            Terreno.Effect.SetValue("colorEspecular", Material.Especular);
            Terreno.Effect.SetValue("exponenteEspecular", Material.Brillito);
            Terreno.Effect.SetValue("colorAmbiente", Material.Ambiente);
            Terreno.Effect.SetValue("posicionCamara", TGCVector3.Vector3ToFloat4Array(camara));
        }

        public void CambiarEfecto(Effect efecto, string technique) {
            Terreno.Effect = efecto;
            Terreno.Technique = "Suelo" + technique;
        }

        public void Dispose() {
            Terreno.Dispose();
        }
    }
}
