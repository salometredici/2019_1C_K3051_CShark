using CShark.Jugador;
using CShark.Model;
using CShark.Utils;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Text;

namespace CShark.UI.HUD
{
    public abstract class ItemsMenus : Menu
    {
        public Drawer2D Drawer;
        public CustomSprite Fondo;
        public BotonCerrar Cerrar;
        public Point PosicionTitulo;
        public TgcText2D Titulo;
        public Player player;
        public int Ancho => Fondo.Bitmap.ImageInformation.Width;

        public void Init(string rutaFondo, string texto)
        {
            Drawer = new Drawer2D();
            CargarFondo(rutaFondo);
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            var posAux = new Point((int)Fondo.Position.X + 29, (int)Fondo.Position.Y + 387);
            PosicionTitulo = posAux;
            Cerrar = new BotonCerrar(Fondo.Position, anchoReal, altoReal);
            Titulo = CargarTexto(Color.White, 20f, texto);
        }
        private TgcText2D CargarTexto(Color color, float size, string titulo)
        {
            var texto = new TgcText2D
            {
                Color = color,
                Position = PosicionTitulo,
                Align = TgcText2D.TextAlign.LEFT,
                Size = new Size(Ancho, 70),
                Text = titulo
            };
            texto.changeFont(new Font("Arial", size, FontStyle.Bold));
            return texto;
        }

        private void CargarFondo(string ruta)
        {
            var device = D3DDevice.Instance.Device;
            var w = device.Viewport.Width;
            var h = device.Viewport.Height;
            var path = Game.Default.MediaDirectory + ruta;
            Fondo = new CustomSprite
            {
                Bitmap = new CustomBitmap(path + "fondo.png", device)
            };
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            Fondo.Position = new TGCVector2((w - anchoReal) / 2f, (h - altoReal) / 2f);
            Fondo.Scaling = new TGCVector2(anchoReal / Fondo.Bitmap.Width, altoReal / Fondo.Bitmap.Height);
        }

        public override void Update(GameModel juego)
        {
            if (player == null)
                player = juego.Player; //esto esta como el orto
            Cerrar.Update(juego);
        }

        public new void Dispose()
        {
            Cerrar.Dispose();
            Titulo.Dispose();
            Fondo.Dispose();
        }
    }
}
