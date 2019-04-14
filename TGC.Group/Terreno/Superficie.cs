using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.Terrain;

namespace TGC.Group.Terreno
{
    public class Superficie
    {
        private List<string> Heightmaps;
        private List<TgcSimpleTerrain> Terrenos;

        private readonly int AnchoGrilla = 3;
        private readonly int AltoGrilla = 3;
        private readonly int TamañoImagen = 64; //64 x 64 px imagen del heightmap
        private readonly float EscalaXZ = 100;
        private readonly int AlturaMar = 800; //fruta
        
        public Superficie() {
            Terrenos = new List<TgcSimpleTerrain>();
            CargarHeightmaps();
            CargarTerrenos();
        }

        public void Update() {
        }

        public void Render() {
            Terrenos.ForEach(t => t.Render());
        }

        public void Dispose() {
            Terrenos.ForEach(t => t.Dispose());
        }

        private void CargarHeightmaps() {
            Heightmaps = new List<string>();
            Heightmaps.Add("superficie1.jpg");
            Heightmaps.Add("superficie2.jpg");
            Heightmaps.Add("superficie3.jpg");
            Heightmaps.Add("superficie4.jpg");
            Heightmaps.Add("superficie5.jpg");
        }

        private void CargarTerrenos() {
            Terrenos = new List<TgcSimpleTerrain>();
            for (int i = 0; i < AnchoGrilla; i++)
            {
                for (int j = 0; j < AltoGrilla; j++)
                {
                    var centro = new TGCVector3(i * (TamañoImagen - 1), AlturaMar, j * (TamañoImagen - 1));
                    var terreno = new TgcSimpleTerrain();
                    int indiceHeightmap = new Random().Next(Heightmaps.Count);
                    string heightmap = Heightmaps.ElementAt(indiceHeightmap);
                    terreno.loadHeightmap(Game.Default.MediaDirectory + "\\Heightmaps\\" + heightmap, EscalaXZ, 1, centro);
                    terreno.loadTexture(Game.Default.MediaDirectory + "\\Textures\\mar.jpg");
                    Terrenos.Add(terreno);
                }
            }
        }
    }
}
