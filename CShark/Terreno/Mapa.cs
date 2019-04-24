using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Terrain;

namespace CShark.Terreno
{
    public class Mapa
    {
        private TGCBox Box;
        private TgcSimpleTerrain Terreno;
        public TGCVector3 Centro;

        public Mapa(TgcSkyBox skybox, TgcSimpleTerrain terreno) {
            Box = TGCBox.fromSize(skybox.Center, skybox.Size);
            Centro = skybox.Center;
            Terreno = terreno;
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Terreno.Position.Y + 40f;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;

        /*public TGCVector3 DistanciaBorde(TGCVector3 punto) {
            var x = punto.X - Centro.X + Box.Size.X / 2f;
            var y = punto.Y - Centro.Y + Box.Size.Y / 2f;
            var z = punto.Z - Centro.Z + Box.Size.Z / 2f;
            return new TGCVector3(x, y, z);
        }*/
    }
}
