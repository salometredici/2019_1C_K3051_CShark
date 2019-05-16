using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;

namespace CShark.Luces.Materiales
{
    public class Madera : IMaterial
    {
        public ColorValue Difuso => ColorValue.FromColor(Color.White);
        public ColorValue Emisivo => ColorValue.FromColor(Color.Black);
        public ColorValue Ambiente => ColorValue.FromColor(Color.White);
        public ColorValue Especular => ColorValue.FromColor(Color.White);
        public float Brillito => 10f;
    }
}
