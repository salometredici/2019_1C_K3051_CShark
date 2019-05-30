using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using TGC.Core.Shaders;
using CShark.Model;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;

namespace CShark.Terreno
{
    public class Superficie {
        public TgcSimpleTerrain Terrain;
        private float Time = 0;
        public float Altura = 18000f;
        private readonly float TamañoHM = 512f;

        public void CargarTerrains() {
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\textura22.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\waveHeightmap3.png";
            Terrain = new TgcSimpleTerrain();
            Terrain.loadTexture(textura);
            Terrain.loadHeightmap(heightmap, 350000 / TamañoHM, 2.25f, TGCVector3.Empty); 
            Terrain.AlphaBlendEnable = true;
        }

        public void Update(GameModel game) {
            Time += game.ElapsedTime;
        }        

        public void Render() {
            ActivarAlphaBlend();
            Terrain.Render();
            DesactivarAlphaBlend();
        }

        public void Dispose() {
            Terrain.Dispose();
        }

        private void ActivarAlphaBlend() {
            if (Terrain.AlphaBlendEnable) {
                D3DDevice.Instance.Device.RenderState.AlphaTestEnable = true;
                D3DDevice.Instance.Device.RenderState.AlphaBlendEnable = true;
            }
        }

        private void DesactivarAlphaBlend() {
            D3DDevice.Instance.Device.RenderState.AlphaTestEnable = false;
            D3DDevice.Instance.Device.RenderState.AlphaBlendEnable = false;
        }

        internal void CambiarEfecto(Effect efecto, string technique) {
            Terrain.Effect = efecto;
            Terrain.Technique = "Olas" + technique;
        }
    }
}
