using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Terrain;
using static Microsoft.DirectX.Direct3D.CustomVertex;

namespace CShark.Terreno
{
    public class Suelo : IDisposable
    {
        private TgcSimpleTerrain Terreno;
        private Effect Effect;

        public bool AlphaBlendEnable => Terreno.AlphaBlendEnable;
        public TGCVector3 Center => Terreno.Center;

        public Suelo() {
            var tamañoHM = 256f;
            var alturaTerreno = 19996.643f; //desde 3ds max para que quede exacto
            var anchoAltoMapa = 150000f;
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno.png";
            var shader = Game.Default.ShadersDirectory + "Sol.fx";
            var xz = anchoAltoMapa / tamañoHM;
            var y = alturaTerreno / HeightmapMethods.AlturaHeightmap(heightmap);
            Effect = TGCShaders.Instance.LoadEffect(shader);
            Terreno = new TgcSimpleTerrain();
            Terreno.loadHeightmap(heightmap, xz, y, TGCVector3.Empty);
            Terreno.loadTexture(textura);
            //Terreno.AlphaBlendEnable = true;
            Terreno.Effect = TGCShaders.Instance.TgcMeshPointLightShader;
            Terreno.Technique = TGCShaders.Instance.GetTGCMeshTechnique(TgcMesh.MeshRenderType.DIFFUSE_MAP);
        }
        
        public void Dispose() {
            Effect.Dispose();
            Terreno.Dispose();
        }

        public void Update(Sol sol, TGCVector3 camara) {
            Terreno.Effect.SetValue("lightColor", ColorValue.FromColor(sol.ColorLuz));
            Terreno.Effect.SetValue("lightPosition", TGCVector3.Vector3ToFloat4Array(sol.Posicion));
            Terreno.Effect.SetValue("eyePosition", TGCVector3.Vector3ToFloat4Array(camara));
            Terreno.Effect.SetValue("lightIntensity", sol.Intensidad);
            Terreno.Effect.SetValue("lightAttenuation", 0.1f);
            Terreno.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
            Terreno.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
            Terreno.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.White));
            Terreno.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
            Terreno.Effect.SetValue("materialSpecularExp", 10f);
        }

        public void Render() {
            Terreno.Render();
        }

        public PositionTextured[] GetData() {
            return Terreno.getData();
        }
    }
}
