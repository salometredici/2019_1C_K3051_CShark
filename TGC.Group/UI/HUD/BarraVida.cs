using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Group.Utils;

namespace TGC.Group.UI.HUD
{
    public class BarraVida : BarraEstado
    {
        public BarraVida(TGCVector2 posicion, int valorMaximo) 
            : base(posicion, valorMaximo, Color.FromArgb(63, 84, 12)) { }

        protected override void CargarBitmap() {
            Fondo = new CustomSprite();
            Barra = new CustomSprite();
            Fondo.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\vidaFondo.png", D3DDevice.Instance.Device);
            Barra.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\vidaBarra.png", D3DDevice.Instance.Device);
        }
    }
}
