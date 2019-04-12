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
using TGC.Group.Utils;

namespace TGC.Group.UI
{
    public class Slider
    {
        public string Nombre { get; set; }
        public float Valor { get; set; }
        public float ValorMinimo { get; set; }
        public float ValorMaximo { get; set; }
        private Drawer2D Drawer;
        private TGCVector2 Posicion; //absoluta
        private int Ancho = 400;
        private int Alto = 20;

        private CustomSprite BarraVacia;
        private CustomSprite BarraLlena;
        private CustomSprite Marcador;
        private int AnchoMarcador = 30;
        private int AltoMarcador = 30;
        private TgcText2D TextoValor;
        public float ValPixel => (ValorMaximo - ValorMinimo) / Ancho;

        public Slider(string nombre, float minimo, float maximo, int x, int y) {
            Valor = minimo;
            ValorMinimo = minimo;
            ValorMaximo = maximo;
            Nombre = nombre;
            Posicion = new TGCVector2(x, y);
            CargarSprites();
            TextoValor.Size = new Size(50, 15);
            TextoValor.Format = Microsoft.DirectX.Direct3D.DrawTextFormat.Left;
            TextoValor.Position = new Point(x, y + 50);
        }

        public void Update(TgcD3dInput input) {
            if (Presionado(input) && DentroDeLimite())
            {
                var mitadMarcador = AnchoMarcador / 2;
                Marcador.Position = new TGCVector2(Cursor.Position.X - mitadMarcador, Marcador.Position.Y);
                var distancia = Marcador.Position.X - Posicion.X;
                BarraLlena.Scaling = new TGCVector2(distancia / BarraLlena.Bitmap.Width, 1);
                Valor = ValorMinimo + ValPixel * (distancia + mitadMarcador);
                TextoValor.Text = Valor.ToString();
            }
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(BarraVacia);
            Drawer.DrawSprite(BarraLlena);
            Drawer.DrawSprite(Marcador);
            Drawer.EndDrawSprite();
            TextoValor.render();
        }

        private bool DentroDeLimite() {
            return Cursor.Position.X > Posicion.X && Cursor.Position.X < Posicion.X + Ancho;
        }

        private bool MouseSobreMarcador() {
            return Cursor.Position.X > Marcador.Position.X &&
            Cursor.Position.X < Marcador.Position.X + AnchoMarcador &&
            Cursor.Position.Y > Marcador.Position.Y &&
            Cursor.Position.Y < Marcador.Position.Y + AltoMarcador;
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseSobreMarcador();
        }

        private void CargarSprites() {
            Drawer = new Drawer2D();
            BarraVacia = new CustomSprite();
            BarraVacia.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\slider1.png", D3DDevice.Instance.Device);
            BarraVacia.Position = Posicion; //relativa 0 pero cambia
            BarraVacia.Scaling = new TGCVector2(Ancho / BarraVacia.Bitmap.Width, 1);
            BarraLlena = new CustomSprite();
            BarraLlena.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\slider2.png", D3DDevice.Instance.Device);
            BarraLlena.Position = Posicion; //relativa 0 
            Marcador = new CustomSprite();
            Marcador.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\slider3.png", D3DDevice.Instance.Device);
            Marcador.Position = Posicion; //relativa 0 pero cambia
            TextoValor = new TgcText2D();
        }
    }
}
