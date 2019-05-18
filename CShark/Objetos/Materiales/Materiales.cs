using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Objetos
{
    public class Materiales
    {
        public static Material Normal => new Material {
            Difuso = ColorValue.FromColor(Color.White),
            Emisivo = ColorValue.FromColor(Color.Black),
            Ambiente = ColorValue.FromColor(Color.White),
            Especular = ColorValue.FromColor(Color.White),
            Brillito = 5f
        };

        public static Material Arena => new Material {
            Difuso = ColorValue.FromColor(Color.White),
            Emisivo = ColorValue.FromColor(Color.FromArgb(255,33, 17, 0)),
            Ambiente = ColorValue.FromColor(Color.White),
            Especular = ColorValue.FromColor(Color.Wheat),
            Brillito = 5f
        };

        public static Material Metal => new Material {
            Difuso = ColorValue.FromColor(Color.White),
            Emisivo = ColorValue.FromColor(Color.FromArgb(255, 53, 53, 53)),
            Ambiente = ColorValue.FromColor(Color.White),
            Especular = ColorValue.FromColor(Color.White),
            Brillito = 50f
        };

        public static Material Madera => new Material {
            Difuso = ColorValue.FromColor(Color.White),
            Emisivo = ColorValue.FromColor(Color.FromArgb(255, 33, 17, 0)),
            Ambiente = ColorValue.FromColor(Color.White),
            Especular = ColorValue.FromColor(Color.White),
            Brillito = 10f
        };

        public static Material Roca => new Material {
            Difuso = ColorValue.FromColor(Color.White),
            Emisivo = ColorValue.FromColor(Color.Black),
            Ambiente = ColorValue.FromColor(Color.DarkGray),
            Especular = ColorValue.FromColor(Color.White),
            Brillito = 100f
        };


    }
}
