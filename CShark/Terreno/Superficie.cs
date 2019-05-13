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

        private TgcSimpleTerrain Terrain;
        private List<WaveTerrain> Waves;
        private float Time = 0;
        private Effect Effect;
        public float Altura = 2800f;

        private readonly float TamañoHM = 512f;

        public Superficie() {
            Effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "WaveShader.fx");
        }

        private float AlturaTerrain() {
            int[,] data = Terrain.HeightmapData;
            float suma = 0f;
            for (int i = 0; i < TamañoHM; i++)
                suma += data[i, i];
            return suma / TamañoHM;
        }

        public void CargarTerrains() {
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\texturita2.jpg";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\waveHeightmap.png";
            Terrain = new TgcSimpleTerrain();
            Terrain.loadTexture(Game.Default.MediaDirectory + @"Mapa\Textures\texturita2.jpg");
            Terrain.loadHeightmap(Game.Default.MediaDirectory + @"Mapa\Textures\waveHeightmap.png", 10000/TamañoHM, 2, TGCVector3.Empty);
            Terrain.AlphaBlendEnable = true;
            Terrain.Effect = Effect;
            Terrain.Technique = "WaveEffect";
        }

        public void Update(float elapsedTime) {
            Time += elapsedTime;
            Effect.SetValue("time", Time);
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

    }
}
