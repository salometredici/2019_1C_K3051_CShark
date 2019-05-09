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

namespace CShark.Terreno
{
    public class Superficie {
        private TgcSimpleTerrain Terrain;
        private float Time = 0;
        private Effect Effect;

        public Superficie() {
            Effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "WaveShader.fx");
        }

        private float Altura() {
            int[,] data = Terrain.HeightmapData;
            float suma = 0f;
            for (int i = 0; i < 512; i++)
                suma += data[i, i];
            return suma / 512f;
        }

        public void CargarTerrains() {
            //var textura = Game.Default.MediaDirectory + @"Mapa\Textures\texturaAgua.png";
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\texturita2.jpg";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\waveHeightmap.png";
            Terrain = new TgcSimpleTerrain();
            Terrain.loadTexture(textura);
            Terrain.loadHeightmap(heightmap, 10000 / 512f, 2, new TGCVector3(0, 0, 0));
            Terrain.loadHeightmap(heightmap, 10000 / 512f, 2, new TGCVector3(0, -Altura() / 2, 0));
            Terrain.AlphaBlendEnable = true;
            Terrain.Effect = Effect;
            Terrain.Technique = "WaveEffect";
        }

        public void Update(float elapsedTime) {
            Time += elapsedTime;
            Effect.SetValue("time", Time);
        }

        public void Render() {
            Terrain.Render();
        }

        public void Dispose() {
            Terrain.Dispose();
        }

    }
}
