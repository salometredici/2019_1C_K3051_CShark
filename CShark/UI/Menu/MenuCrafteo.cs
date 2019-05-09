using CShark.Model;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;

namespace CShark.UI
{
    public class MenuCrafteo : Menu
    {
        private Drawer2D Drawer;
        private CustomSprite Fondo;
        private BotonCerrar Cerrar;
        private BotonCraftear Boton;
        private List<BotonItem> Botones;
        private Puntero puntero;

        public MenuCrafteo() {
            Drawer = new Drawer2D();
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
            Cerrar = new BotonCerrar(Fondo.Position, anchoReal, altoReal);
            Boton = new BotonCraftear(Fondo.Position, anchoReal, altoReal);
            puntero = new Puntero();
        }

        public override void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Cerrar.Render(Drawer);
            Boton.Render(Drawer);
            Drawer.EndDrawSprite();
            puntero.Render();
        }

        public override void Update(GameModel juego) {
            puntero.Update();
            Cerrar.Update(juego);
        }
    }
}
