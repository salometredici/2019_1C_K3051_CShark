using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace CShark.Luces.Materiales
{
    public class Arena : IMaterial
    {
        public ColorValue Difuso => ColorValue.FromColor(Color.White);
        public ColorValue Emisivo => ColorValue.FromColor(Color.Black);
        public ColorValue Ambiente => ColorValue.FromColor(Color.White);
        public ColorValue Especular => ColorValue.FromColor(Color.White);
        public float Brillito => 5f;
    }
}
