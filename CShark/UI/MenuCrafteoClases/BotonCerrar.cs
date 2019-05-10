using CShark.Model;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
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
    public class BotonCerrar : IDisposable
    {
        private CustomSprite Sprite;
        public TGCVector2 Posicion => Sprite.Position;
        public float Ancho;
        public float Alto;
        public bool Cerrar = false;

        public BotonCerrar(TGCVector2 posicionMenu, float ancho, float alto) {
            Sprite = new CustomSprite();
            Sprite.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + @"MenuCrafteo\cerrar.png", D3DDevice.Instance.Device);
            float anchoReal = Sprite.Bitmap.ImageInformation.Width;
            float altoReal = Sprite.Bitmap.ImageInformation.Height;
            Ancho = anchoReal;
            Alto = altoReal;
            Sprite.Scaling = new TGCVector2(anchoReal / Sprite.Bitmap.Width, altoReal / Sprite.Bitmap.Height);
            Sprite.Position = posicionMenu + new TGCVector2(ancho - Ancho/2f, -Alto/2f);
        }

        public void Update(GameModel game) {
            if (Presionado(game.Input)) {
                game.GameManager.SwitchMenu(game);
            }
        }

        public void Render(Drawer2D drawer) {
            drawer.DrawSprite(Sprite);
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

        public void Dispose() {
            Sprite.Dispose();
        }
    }
}
