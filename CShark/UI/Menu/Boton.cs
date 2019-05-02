using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Text;
using System.Drawing;
using CShark.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using System.Windows.Forms;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;
using CShark.Model;
using Font = System.Drawing.Font;

namespace CShark.UI
{
    public class Boton
    {
        protected CustomSprite Fondo;
        public Point Posicion;
        protected TgcText2D Texto;
        protected Drawer2D Drawer;
        protected Action<GameModel> Accion;

        protected CustomBitmap FondoNormal;
        protected CustomBitmap FondoSeleccionado;
        protected int Ancho = 400;
        protected int Alto = 75;

        public Boton(string texto, int x, int y, Action<GameModel> accion) {
            Accion = accion;
            Drawer = new Drawer2D();
            Posicion = new Point(x - Ancho / 2, y + Alto / 2);
            CargarFondo();
            CargarTexto(texto);
        }

        public Boton(CustomSprite sprite, string texto, int x, int y, Action<GameModel> accion)
        {
            Accion = accion;
            Drawer = new Drawer2D();
            Posicion = new Point(x, y);
            Fondo = sprite;
            FondoNormal = sprite.Bitmap;
            FondoSeleccionado = sprite.Bitmap;
        }

        public void Update(GameModel juego) {
            Fondo.Bitmap = MouseAdentro() ? FondoSeleccionado : FondoNormal;
            if (Presionado(juego.Input))
                Accion.Invoke(juego);
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseAdentro();
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.EndDrawSprite();
            Texto.render();
        }

        private void CargarFondo() {
            FondoNormal = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton1.png", D3DDevice.Instance.Device);
            FondoSeleccionado = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton2.png", D3DDevice.Instance.Device);
            Fondo = new CustomSprite();
            Fondo.Bitmap = FondoNormal;
            Fondo.Position = new TGCVector2(Posicion.X, Posicion.Y);
            Fondo.Scaling = new TGCVector2((float)Ancho / FondoNormal.Width, (float)Alto / FondoNormal.Height);
        }

        public virtual void CargarTexto(string texto) {
            Texto = new TgcText2D
            {
                Color = Color.White,
                Text = texto,
                Size = new Size(Ancho, Alto),
                Format = DrawTextFormat.Center,
                Position = Posicion
            };
            Texto.changeFont(new Font("Arial", 25, FontStyle.Bold, GraphicsUnit.Pixel));
            Texto.Position = new Point(Texto.Position.X, Texto.Position.Y + (Alto - 38) / 2);
        }

        private bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }
    }
}
