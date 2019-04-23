using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Text;
using CShark.Utils;

namespace CShark.UI.HUD
{
    public class PantallaMuerte
    {
        private Drawer2D Drawer;
        private CustomSprite Fondo;
        private CustomSprite Franja;
        private CustomSprite Texto;
        private TgcText2D TextoPresione;

        public PantallaMuerte() {
            Drawer = new Drawer2D();
            CargarSprites();
            PosicionarSprites();
        }

        private void CargarSprites() {
            Fondo = new CustomSprite();
            Franja = new CustomSprite();
            Texto = new CustomSprite();
            TextoPresione = new TgcText2D();
            Fondo.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\Muerte\\fondo.png", D3DDevice.Instance.Device);
            Franja.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\Muerte\\franja.png", D3DDevice.Instance.Device);
            Texto.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\HUD\\Muerte\\texto.png", D3DDevice.Instance.Device);
        }

        private void PosicionarSprites() {
            float ancho = D3DDevice.Instance.Device.Viewport.Width;
            float alto = D3DDevice.Instance.Device.Viewport.Height;
            Fondo.Scaling = new TGCVector2(ancho / Fondo.Bitmap.Width, alto / Fondo.Bitmap.Height);
            Franja.Scaling = new TGCVector2(ancho / Franja.Bitmap.Width, 240f / Franja.Bitmap.Height);
            Franja.Position = new TGCVector2(0, (alto - Franja.Bitmap.Height) / 2);
            var anchoTexto = 551f;
            var altoTexto = 80f;
            Texto.Scaling = new TGCVector2(anchoTexto / Texto.Bitmap.Width, altoTexto / Texto.Bitmap.Height);
            Texto.Position = new TGCVector2((ancho - anchoTexto) / 2, (alto - altoTexto) / 2);
            TextoPresione.Text = "Presione ESC para comenzar de nuevo";
            TextoPresione.Align = TgcText2D.TextAlign.CENTER;
            TextoPresione.Color = Color.White;
            TextoPresione.Size = new Size((int)ancho, 50);
            TextoPresione.Position = new Point(0, (int)Texto.Position.Y + (int)altoTexto + 100);
            TextoPresione.changeFont(new Font("Arial", 16));
        }

        public void Update() {

        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.DrawSprite(Franja);
            Drawer.DrawSprite(Texto);
            Drawer.EndDrawSprite();
            TextoPresione.render();
        }
    }
}
