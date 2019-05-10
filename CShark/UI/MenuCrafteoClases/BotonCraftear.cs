using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;

namespace CShark.UI
{
    public class BotonCraftear : IDisposable
    {
        private CustomBitmap BitmapNormal;
        private CustomBitmap BitmapFocus;
        private CustomSprite Sprite;
        public float Ancho;
        public float Alto;
        public TGCVector2 Posicion => Sprite.Position;

        public BotonCraftear(TGCVector2 posicionMenu, float ancho, float alto) {
            Sprite = new CustomSprite();
            BitmapNormal = CargarBitmap("craft");
            BitmapFocus = CargarBitmap("craftFocus");
            Sprite.Bitmap = BitmapNormal;
            Ancho = Sprite.Bitmap.ImageInformation.Width;
            Alto = Sprite.Bitmap.ImageInformation.Height;
            Sprite.Scaling = new TGCVector2(Ancho / Sprite.Bitmap.Width, Alto / Sprite.Bitmap.Height);
            Sprite.Position = posicionMenu + new TGCVector2(ancho - Ancho - 10, alto - Alto - 10);
        }

        private CustomBitmap CargarBitmap(string bm) {
            return new CustomBitmap(Game.Default.MediaDirectory + @"MenuCrafteo\" + bm + ".png", D3DDevice.Instance.Device);
        }

        public void Update(MenuCrafteo menu, TgcD3dInput input) {
            if (MouseAdentro()) Sprite.Bitmap = BitmapFocus;
            else Sprite.Bitmap = BitmapNormal;
            if (Presionado(input)) 
                menu.Craftear();
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseAdentro();
        }

        private bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }

        public void Render(Drawer2D drawer) {
            drawer.DrawSprite(Sprite);
        }

        public void Dispose() {
            Sprite.Dispose();
        }
    }
}
