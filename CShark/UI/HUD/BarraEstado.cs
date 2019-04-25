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
    public abstract class BarraEstado
    {
        public TGCVector2 Posicion { get; private set; }

        private Drawer2D Drawer;
        private TgcText2D Texto;
        private TgcText2D SombraTexto;
        protected CustomSprite Fondo;
        protected CustomSprite Barra;
        private float ValorMaximo;

        private readonly int AnchoBarra = 626 / 2;
        private readonly int AltoBarra = 126 / 2;

        public BarraEstado(TGCVector2 posicion, float valorMaximo, Color colorSombra) {
            Posicion = posicion;
            ValorMaximo = valorMaximo;
            Drawer = new Drawer2D();
            CargarBitmap();
            CargarSprites();
            CargarTexto(colorSombra);
        }

        protected abstract void CargarBitmap();

        private void CargarSprites() {
            var escala = new TGCVector2((float)AnchoBarra / Barra.Bitmap.Width, (float)AltoBarra / Barra.Bitmap.Height);
            Barra.Position = new TGCVector2(Posicion.X, Posicion.Y);
            Fondo.Position = new TGCVector2(Posicion.X, Posicion.Y);
            Barra.Scaling = escala;
            Fondo.Scaling = escala;
        }

        private void CargarTexto(Color colorSombra) {
            var fuente = new Font("Arial", 20, FontStyle.Bold);
            Texto = new TgcText2D();
            Texto.Color = Color.White;
            Texto.Align = TgcText2D.TextAlign.CENTER;
            Texto.Size = new Size(AnchoBarra, AltoBarra);
            Texto.Position = new Point((int)Posicion.X, (int)Posicion.Y + 32 / 2);
            Texto.changeFont(fuente);
            Texto.Text = "100 %";
            SombraTexto = new TgcText2D();
            SombraTexto.Color = colorSombra;
            SombraTexto.Align = Texto.Align;
            SombraTexto.Size = Texto.Size;
            SombraTexto.Position = new Point(Texto.Position.X + 2, Texto.Position.Y + 2); //desplazamiento sombra
            SombraTexto.changeFont(fuente);
            SombraTexto.Text = Texto.Text;
        }

        public void Update(float valor) {
            int porcentaje = (int)Math.Round((double)valor / ValorMaximo * 100);
            Texto.Text = porcentaje + " %";
            SombraTexto.Text = Texto.Text;
            int anchoMostrar = Barra.Bitmap.Width * porcentaje / 100;
            Barra.SrcRect = new Rectangle(0, 0, anchoMostrar, Barra.Bitmap.Height);
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.DrawSprite(Barra);
            Drawer.EndDrawSprite();
            SombraTexto.render();
            Texto.render();
        }
    }
}
