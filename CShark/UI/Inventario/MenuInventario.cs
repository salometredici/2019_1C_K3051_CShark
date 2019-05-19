using CShark.Items;
using CShark.Items.Crafteables;
using CShark.Items.Recolectables;
using CShark.Jugador;
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
    public class MenuInventario : Menu, IDisposable
    {
        private Drawer2D Drawer;
        private CustomSprite Fondo;
        private BotonCerrar Cerrar;
        private List<BotonInventario> Botones;
        public Point PosicionTitulo;
        public TgcText2D Titulo;
        private string Seleccionado;

        private Player player;

        public int Ancho => Fondo.Bitmap.ImageInformation.Width;

        public MenuInventario()
        {
            Drawer = new Drawer2D();
            CargarFondo();
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            var posAux = new Point((int)Fondo.Position.X + 29, (int)Fondo.Position.Y + 387);
            PosicionTitulo = posAux;
            Cerrar = new BotonCerrar(Fondo.Position, anchoReal, altoReal);
            CargarBotones(Fondo.Position);
            Titulo = CargarTexto(Color.White, 20f);
        }

        private TgcText2D CargarTexto(Color color, float size)
        {
            var texto = new TgcText2D
            {
                Color = color,
                Position = PosicionTitulo,
                Align = TgcText2D.TextAlign.LEFT,
                Size = new Size(Ancho, 70),
                Text = ""
            };
            texto.changeFont(new Font("Arial", size, FontStyle.Bold));
            return texto;
        }

        private void CargarFondo()
        {
            var device = D3DDevice.Instance.Device;
            var w = device.Viewport.Width;
            var h = device.Viewport.Height;
            var path = Game.Default.MediaDirectory + @"Menu\Inventario\";
            Fondo = new CustomSprite();
            Fondo.Bitmap = new CustomBitmap(path + "fondo.png", device);
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            Fondo.Position = new TGCVector2((w - anchoReal) / 2f, (h - altoReal) / 2f);
            Fondo.Scaling = new TGCVector2(anchoReal / Fondo.Bitmap.Width, altoReal / Fondo.Bitmap.Height);
        }

        private void CargarBotones(TGCVector2 posInicial)
        {
            var pos = posInicial + new TGCVector2(29, 29);
            var desplazamColumna = new TGCVector2(179, 0);
            var desplazamFila = new TGCVector2(-358, 179);
            Botones = new List<BotonInventario>();
            Botones.Add(new BotonInventario("Arpon", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Oxigeno", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Medkit", pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario("Oro", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Plata", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Hierro", pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario("Wumpa", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Coral", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Pez", pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario("Chip", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Pila", pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario("Burbuja",pos));
        }

        public void CambiarItem(BotonInventario boton)
        {
            Botones.ForEach(b => b.Seleccionado = false);
            boton.Seleccionado = true;
            Seleccionado = boton.Titulo;
        }

        public override void Render()
        {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Cerrar.Render(Drawer);
            Botones.ForEach(b => b.Render(Drawer));
            Drawer.EndDrawSprite();
            Titulo.render();
        }

        public override void Update(GameModel juego)
        {
            if (player == null)
                player = juego.Player; //esto esta como el orto
            Cerrar.Update(juego);
            Botones.ForEach(b => b.Update(juego, this));
            if (Seleccionado != null)
                Titulo.Text = TituloDisplay();
        }
        private string TituloDisplay()
        {
            switch (Seleccionado)
            {
                default:
                    return Seleccionado;
            }
        }

        public new void Dispose()
        {
            Cerrar.Dispose();
            Titulo.Dispose();
            Fondo.Dispose();
            Botones.ForEach(b => b.Dispose());
        }
    }
}
