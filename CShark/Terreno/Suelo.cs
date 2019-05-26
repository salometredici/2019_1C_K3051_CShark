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
        private TgcSimpleTerrain Terreno;

        public bool AlphaBlendEnable => Terreno.AlphaBlendEnable;
        public TGCVector3 Center => Terreno.Center;

        private Material Material;
        private Texture TexturaRayoSol;

        public Suelo() {
            var tamañoHM = 1024f;
            var alturaTerreno = 20000f; //desde 3ds max para que quede exacto
            var anchoAltoMapa = 350000f;
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno2.png";
            var xz = anchoAltoMapa / tamañoHM;
            var y = alturaTerreno / HeightmapMethods.AlturaHeightmap(heightmap);
            Terreno = new TgcSimpleTerrain();
            Terreno.loadHeightmap(heightmap, xz, y, TGCVector3.Empty);
            Terreno.loadTexture(textura);
            Material = Materiales.Arena;
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\fondo.png";
            TexturaRayoSol = TgcTexture.createTexture(D3DDevice.Instance.Device, path).D3dTexture;
        }

        float time = 0;

        public void Dispose() {
            Terreno.Dispose();
        }

        public void Update(float elapsedTime, TGCVector3 camara) {
            time += elapsedTime;
            Efectos.Instancia.ActualizarLuces(Terreno.Effect, Material, camara);
            Terreno.Effect.SetValue("texRayosSol", TexturaRayoSol);
            Terreno.Effect.SetValue("time", time);
        }

        public void Render() {
            Terreno.Render();
        }

        public PositionTextured[] GetData() {
            return Terreno.getData();
        }

        public void CambiarEfecto(Effect efecto, string technique) {
            Terreno.Effect = efecto;
            Terreno.Technique = "Suelo" + technique;
        }
    }
}
