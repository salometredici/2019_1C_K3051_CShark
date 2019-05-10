using CShark.Items;
using CShark.Model;
using CShark.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Text;

namespace CShark.UI
{
    public class BotonItem : IDisposable
    {
        private CustomBitmap FondoNormal;
        private CustomBitmap FondoHover;
        private CustomBitmap FondoClick;       
        public ICrafteable Item;
        private CustomSprite Fondo;
        public bool Seleccionado = false;
        public TGCVector2 Posicion => Fondo.Position;
        public string Titulo;

        public int Ancho => Fondo.Bitmap.ImageInformation.Width;
        public int Alto => Fondo.Bitmap.ImageInformation.Height;

        public BotonItem(ICrafteable item, TGCVector2 posicion, MenuCrafteo menu) {
            Item = item;
            Titulo = item.Tipo.ToString();
            FondoNormal = CargarBitmap(Titulo);
            FondoHover = CargarBitmap(Titulo + "Hover");
            FondoClick = CargarBitmap(Titulo + "Click");
            Fondo = CargarSprite(posicion);
        }

        private CustomSprite CargarSprite(TGCVector2 posicion) {
            var sprite = new CustomSprite();
            sprite.Position = posicion;
            sprite.Bitmap = FondoNormal;
            float anchoReal = sprite.Bitmap.ImageInformation.Width;
            float altoReal = sprite.Bitmap.ImageInformation.Height;
            sprite.Scaling = new TGCVector2(anchoReal / sprite.Bitmap.Width, altoReal / sprite.Bitmap.Height);
            return sprite;
        }

        private CustomBitmap CargarBitmap(string nombre) {
            var path = Game.Default.MediaDirectory + @"MenuCrafteo\Items\";
            return new CustomBitmap(path + nombre + ".png", D3DDevice.Instance.Device);
        }        

        public void Update(GameModel juego, MenuCrafteo menu) {
            Fondo.Bitmap = MouseAdentro() ? FondoHover : FondoNormal;
            if (Seleccionado)
                Fondo.Bitmap = FondoClick;
            if (Presionado(juego.Input))
                menu.CambiarItem(this);
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseAdentro();
        }

        public void Render(Drawer2D Drawer) {
            Drawer.DrawSprite(Fondo);
        }

        private bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }

        public void Dispose() {
            FondoNormal.Dispose();
            FondoHover.Dispose();
            FondoClick.Dispose();
            Fondo.Dispose();
        }
    }
}
