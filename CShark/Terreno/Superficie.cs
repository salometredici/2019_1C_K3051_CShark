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
        private Effect Effect;
        public float Altura = 18000f;

        private readonly float TamañoHM = 512f;

        public Superficie() {
            Effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "WaveShader.fx");
        }

        public void CargarTerrains() {
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\wave1.jpg";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\waveHeightmap3.png";
            Terrain = new TgcSimpleTerrain();
            Terrain.loadTexture(textura);
            //por ahora dejalo asi esto, la otra opcion es cargar un heightmap
            //gigante para que quede bien, pero el D3d es una verga y se queda sin memoria
            //al cargar
            Terrain.loadHeightmap(heightmap, 300000 / TamañoHM, 2.25f, TGCVector3.Empty); 
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
