using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;

namespace CShark.Utils
{
    public static class HeightmapMethods
    {
        public static float AlturaHeightmap(string path) {
            var bitmap = (Bitmap)Image.FromFile(path);
            var tamanio = bitmap.Size.Width; //son cuadrados
            int altura = 0;
            for (var i = 0; i < tamanio; i++) {
                for (var j = 0; j < tamanio; j++) {
                    var intensidad = bitmap.GetPixel(j, i).R; //usamos Heightmaps blanco y negro, RGB son iguales
                    altura = intensidad > altura ? intensidad : altura;
                }
            }
            return altura;
        }
    }
}
