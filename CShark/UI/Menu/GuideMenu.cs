using CShark.Items;
using CShark.Items.Crafteables;
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
    public class GuideMenu : Menu, IDisposable
    {
        private Drawer2D Drawer;
        private CustomSprite Fondo;
        private BotonCerrar Cerrar;
        public TGCVector2 PosicionTitulo;
        public CustomSprite Titulo;
        public List<TgcText2D> Requerimientos;

        private float anchoReal;
        private float altoReal;

        public int Ancho => Fondo.Bitmap.ImageInformation.Width;

        //Copiadisimo de tu menú del craft, después voy a intentar que no se repita tanto el código
        public GuideMenu()
        {
            Drawer = new Drawer2D();
            CargarFondo();
            PosicionTitulo = new TGCVector2(Fondo.Position.X + Fondo.Position.X / 4, Fondo.Position.Y + 25);
            Cerrar = new BotonCerrar(Fondo.Position, anchoReal, altoReal);
            Titulo = new CustomSprite();
            SetItem(Titulo, Game.Default.MediaDirectory + "\\Menu\\controls-title.png", PosicionTitulo, new TGCVector2(0.7f, 0.5f));
            Requerimientos = ArmarRequerimientos();
        }

        private void CargarFondo()
        {
            var device = D3DDevice.Instance.Device;
            var path = Game.Default.MediaDirectory + @"MenuCrafteo\";
            Fondo = new CustomSprite
            {
                Bitmap = new CustomBitmap(path + "fondo.png", device)
            };
            anchoReal = Fondo.Bitmap.ImageInformation.Width;
            altoReal = Fondo.Bitmap.ImageInformation.Height;
            Fondo.Position = new TGCVector2((GameModel.DeviceWidth - anchoReal) / 2f, (GameModel.DeviceHeight - altoReal) / 2f);
            Fondo.Scaling = new TGCVector2(anchoReal / Fondo.Bitmap.Width, altoReal / Fondo.Bitmap.Height);
        }

        public override void Render()
        {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.DrawSprite(Titulo);
            Cerrar.Render(Drawer);
            Drawer.EndDrawSprite();
            Requerimientos.ForEach(r => r.render());
        }

        public override void Update(GameModel juego)
        {
            Cerrar.Update(juego);
        }

        private List<TgcText2D> ArmarRequerimientos()
        {
            var texto = string.Empty;
            var lineas = new List<TgcText2D>();
            var pos = new Point((int)Fondo.Position.X, (int)Fondo.Position.Y + 165);
            foreach(var renglon in Constants.GuideLines) {
                var linea = new TgcText2D
                {
                    Align = TgcText2D.TextAlign.CENTER,
                    Position = pos,
                    Size = new Size(Ancho, 20)
                };
                linea.changeFont(new Font("Arial", 16f, FontStyle.Bold));
                linea.Color = Color.Yellow;
                linea.Text = renglon;
                lineas.Add(linea);
                pos = new Point(pos.X, pos.Y + 30);
            }
            return lineas;
        }

        public void Dispose()
        {
            Cerrar.Dispose();
            Titulo.Dispose();
            Fondo.Dispose();
            Requerimientos.ForEach(r => r.Dispose());
        }
    }
}
