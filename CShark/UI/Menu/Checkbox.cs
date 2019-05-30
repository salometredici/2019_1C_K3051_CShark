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
using TGC.Core.Text;
using CShark.Model;
using CShark.Utils;
using CShark.Variables;
using Font = System.Drawing.Font;

namespace CShark.UI
{
    public class Checkbox
    {
        private CustomSprite Fondo;
        private CustomSprite Tick;
        public Point Posicion;
        private TgcText2D Texto;
        private Drawer2D Drawer;
        private Variable<bool> Variable;

        private CustomBitmap FondoNormal;
        private CustomBitmap FondoHover;
        private int Ancho = 50;
        private int Alto = 50;
        private int LargoTexto = 400;

        public Checkbox(Variable<bool> variable, int x, int y) {
            Variable = variable;
            Drawer = new Drawer2D();
            Posicion = new Point(x, y);
            CargarFondo();
            CargarTexto(variable.Nombre);
        }

        public void Update(TgcD3dInput input) {
            Fondo.Bitmap = MouseAdentro() || Variable.Valor ? FondoHover : FondoNormal;
            if (Presionado(input))
                Variable.Actualizar(!Variable.Valor);
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseAdentro();
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            if (Variable.Valor)
                Drawer.DrawSprite(Tick);
            Drawer.EndDrawSprite();
            Texto.render();
        }

        private void CargarFondo() {
            FondoNormal = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\checkNormal.png", D3DDevice.Instance.Device);
            FondoHover = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\checkHover.png", D3DDevice.Instance.Device);
            Fondo = new CustomSprite();
            Fondo.Bitmap = FondoNormal;
            Fondo.Position = new TGCVector2(Posicion.X, Posicion.Y);
            Fondo.Scaling = new TGCVector2((float)Ancho / FondoNormal.Width, (float)Alto / FondoNormal.Height);
            Tick = new CustomSprite();
            Tick.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\checkTick.png", D3DDevice.Instance.Device);
            Tick.Scaling = new TGCVector2((float)Ancho / Tick.Bitmap.Width, (float)Alto / Tick.Bitmap.Height);
            Tick.Position = new TGCVector2(Posicion.X + 5, Posicion.Y - 5);
        }

        private void CargarTexto(string texto) {
            Texto = new TgcText2D
            {
                Color = Color.White,
                Text = texto,
                Size = new Size(LargoTexto, Alto),
                Format = DrawTextFormat.VerticalCenter,
                Align = TgcText2D.TextAlign.LEFT,
                Position = new Point(Posicion.X + Ancho + 15, Posicion.Y) //desplazado a la derecha
            };
            Texto.changeFont(new Font("Arial", Alto / 2, FontStyle.Bold, GraphicsUnit.Pixel));
        }

        private bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }
    }
}
