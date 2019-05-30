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
        //private TgcSimpleTerrain Terreno;
        private TgcMesh Terreno;

        public bool AlphaBlendEnable => Terreno.AlphaBlendEnable;
        //public TGCVector3 Center => Terreno.Center;
        public TGCVector3 Center => TGCVector3.Empty;
        private Material Material;
        private Texture TexturaRayoSol;
        float time = 0;

        public Suelo() {
            var tamañoHM = 1024f;
            var alturaTerreno = 20000f; //desde 3ds max para que quede exacto
            var anchoAltoMapa = 350000f;
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno2.png";
            var xz = anchoAltoMapa / tamañoHM;
            var y = alturaTerreno / HeightmapMethods.AlturaHeightmap(heightmap);
            Terreno = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Terreno-TgcScene.xml").Meshes[0];
            /*Terreno = new TgcSimpleTerrain();
            Terreno.loadHeightmap(heightmap, xz, y, TGCVector3.Empty);
            Terreno.loadTexture(textura);*/
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

        public PositionTextured[] GetData() {
            var tamañoHM = 1024f;
            var alturaTerreno = 20000f; //desde 3ds max para que quede exacto
            var anchoAltoMapa = 350000f;
            var t = new TgcSimpleTerrain();
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno2.png";
            var xz = anchoAltoMapa / tamañoHM;
            var y = alturaTerreno / HeightmapMethods.AlturaHeightmap(heightmap);
            t.loadHeightmap(heightmap, xz, y, TGCVector3.Empty);
            t.loadTexture(textura);
            return t.getData();
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
