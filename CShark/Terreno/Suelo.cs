using CShark.Luces;
using CShark.Luces.Materiales;
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

namespace CShark.Terreno
{
    public class Suelo : IDisposable
    {
        private TgcSimpleTerrain Terreno;

        public bool AlphaBlendEnable => Terreno.AlphaBlendEnable;
        public TGCVector3 Center => Terreno.Center;

        private Effect Efecto;
        private IMaterial Material;

        public Suelo() {
            var tamañoHM = 256f;
            var alturaTerreno = 19996.643f; //desde 3ds max para que quede exacto
            var anchoAltoMapa = 150000f;
            var textura = Game.Default.MediaDirectory + @"Mapa\Textures\arena.png";
            var heightmap = Game.Default.MediaDirectory + @"Mapa\Textures\terreno.png";
            var xz = anchoAltoMapa / tamañoHM;
            var y = alturaTerreno / HeightmapMethods.AlturaHeightmap(heightmap);
            Terreno = new TgcSimpleTerrain();
            Terreno.loadHeightmap(heightmap, xz, y, TGCVector3.Empty);
            Terreno.loadTexture(textura);
            Efecto = Iluminacion.EfectoLuz;

            //Efecto = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Suelo.fx");
            Terreno.Effect = Efecto;
            //Terreno.Technique = "SueloEffect";
            Terreno.Technique = "Iluminado";
            Material = new Arena();
            var path = Game.Default.MediaDirectory + @"Mapa\Textures\fondo.png";
            tex = TgcTexture.createTexture(D3DDevice.Instance.Device, path).D3dTexture;
        }

        Texture tex;
        
        public void Dispose() {
            Terreno.Dispose();
        }

        public void Update(TGCVector3 camara) {
            Iluminacion.ActualizarEfecto(Efecto, Material, camara);
        }

        public void Render() {
            //Terreno.Effect.SetValue("texx", tex);
            Terreno.Render();
        }

        public PositionTextured[] GetData() {
            return Terreno.getData();
        }
    }
}
