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
    public class BarraOxigeno : BarraEstado
    {
        public BarraOxigeno(TGCVector2 posicion, int valorMaximo)
            : base(posicion, valorMaximo, Color.FromArgb(36, 70, 94)) { }

        protected override void CargarBitmap() {
            Fondo = new CustomSprite();
            Barra = new CustomSprite();
            Fondo.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\oxigenoFondo.png", D3DDevice.Instance.Device);
            Barra.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\oxigenoBarra.png", D3DDevice.Instance.Device);
        }
    }
}
