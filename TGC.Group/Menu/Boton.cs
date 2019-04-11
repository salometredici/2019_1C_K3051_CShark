using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Text;
using System.Drawing;
using TGC.Group.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using System.Windows.Forms;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;

namespace TGC.Group.Menu
{
    public class Boton
    {
        private CustomSprite Fondo;
        private bool Seleccionado = false;
        private Point Posicion;
        private TgcText2D Texto;

        private CustomBitmap FondoNormal;
        private CustomBitmap FondoSeleccionado;
        private int Ancho;
        private int Alto;

        public Boton(string texto, int x, int y) {
            CargarFondo();
            var centradoX = x - Ancho / 2;
            var centradoY = y + Alto / 2;
            Posicion = new Point(centradoX, centradoY);
            Fondo.Position = new TGCVector2(centradoX, centradoY);
            Texto = new TgcText2D
            {
                Align = TgcText2D.TextAlign.CENTER,
                Color = Color.White,
                Text = texto,
                Size = new Size(400, 75),
                Format = DrawTextFormat.Center,
                Position = Posicion
            };
        }

        public void Update(TgcD3dInput input) {
            Fondo.Bitmap = MouseAdentro(input) ? FondoSeleccionado : FondoNormal;
        }

        public void Render(Drawer2D drawer) {
            drawer.DrawSprite(Fondo);
            Texto.render();
        }

        private void CargarFondo() {
            FondoNormal = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton1.png", D3DDevice.Instance.Device);
            FondoSeleccionado = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton2.png", D3DDevice.Instance.Device);
            Ancho = 400;
            Alto = 75;
            Fondo = new CustomSprite();
            Fondo.Bitmap = FondoNormal;
            Fondo.Scaling = new TGCVector2((float)Ancho / FondoNormal.Width, (float)Alto / FondoNormal.Height);
        }

        private bool MouseAdentro(TgcD3dInput input) {
            return input.Xpos > Posicion.X &&
            input.Xpos < Posicion.X + Ancho &&
            input.Ypos > Posicion.Y &&
            input.Ypos < Posicion.Y + Alto;
        }
    }
}
