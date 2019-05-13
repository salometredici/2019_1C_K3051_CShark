using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Terrain;

namespace CShark.Terreno
{
    public static class TerrainExtension
    {
        public static TgcSimpleTerrain Clone(this TgcSimpleTerrain terrain) {
            var t = new TgcSimpleTerrain();
            return t;
        }
    }
}
