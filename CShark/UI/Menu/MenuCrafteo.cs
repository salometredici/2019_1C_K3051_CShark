using CShark.Items;
using CShark.Model;
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
using TGC.Core.Text;

namespace CShark.UI
{
    public class MenuCrafteo : Menu
    {
        private Drawer2D Drawer;
        private CustomSprite Fondo;
        private BotonCerrar Cerrar;
        private BotonCraftear Boton;
        private List<BotonItem> Botones;
        public Point PosicionTitulo;
        public TgcText2D Titulo;
        public TgcText2D TituloHover;
        private BotonItem Seleccionado;

        public int Ancho => Fondo.Bitmap.ImageInformation.Width;

        public MenuCrafteo() {
            Drawer = new Drawer2D();
            CargarFondo();
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            var posAux = Fondo.Position + new TGCVector2(0, 200);
            PosicionTitulo = new Point((int)posAux.X, (int)posAux.Y);
            Cerrar = new BotonCerrar(Fondo.Position, anchoReal, altoReal);
            Boton = new BotonCraftear(Fondo.Position, anchoReal, altoReal);
            CargarBotones(Fondo.Position);
            Titulo = CargarTexto(Color.White, 36f);
            TituloHover = CargarTexto(Color.Wheat, 28f);
        }

        private TgcText2D CargarTexto(Color color, float size) {
            var texto = new TgcText2D {
                Color = color,
                Position = PosicionTitulo,
                Align = TgcText2D.TextAlign.CENTER,
                Size = new Size(Ancho, 70),
                Text = ""
            };
            texto.changeFont(new Font("Arial", size, FontStyle.Bold));
            return texto;
        }

        private void CargarFondo() {
            var device = D3DDevice.Instance.Device;
            var w = device.Viewport.Width;
            var h = device.Viewport.Height;
            var path = Game.Default.MediaDirectory + @"MenuCrafteo\";
            Fondo = new CustomSprite();
            Fondo.Bitmap = new CustomBitmap(path + "fondo.png", device);
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            Fondo.Position = new TGCVector2((w - anchoReal) / 2f, (h - altoReal) / 2f);
            Fondo.Scaling = new TGCVector2(anchoReal / Fondo.Bitmap.Width, altoReal / Fondo.Bitmap.Height);
        }

        private void CargarBotones(TGCVector2 posInicial) {
            var pos = posInicial + new TGCVector2(29, 29);
            var posTitulo = posInicial + new TGCVector2(0, 200);
            Botones = new List<BotonItem>();
            Botones.Add(new BotonItem(Crafteable.Tanque, pos, this));
            pos += new TGCVector2(179, 0);
            Botones.Add(new BotonItem(Crafteable.Arpon, pos, this));
            pos += new TGCVector2(179, 0);
            Botones.Add(new BotonItem(Crafteable.Vida, pos, this));
        }

        public void CambiarItem(BotonItem boton) {
            Botones.ForEach(b => b.Seleccionado = false);
            boton.Seleccionado = true;
            Seleccionado = boton;
        }

        public override void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Cerrar.Render(Drawer);
            Boton.Render(Drawer);
            Botones.ForEach(b => b.Render(Drawer));
            Drawer.EndDrawSprite();
            if (mostrarHover) TituloHover.render();
            else Titulo.render();
            mostrarHover = false;
        }

        public override void Update(GameModel juego) {
            Cerrar.Update(juego);
            Botones.ForEach(b => b.Update(juego, this));
            if (Seleccionado != null)
                Titulo.Text = Seleccionado.Titulo;
        }

        private bool mostrarHover = false;

        public void MostrarTituloHover(string titulo) {
            TituloHover.Text = titulo;
            mostrarHover = true;
        }
    }
}
