using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model;
using TGC.Group.Utils;

namespace TGC.Group.Menu
{
    public class MenuPrincipal
    {
        private Drawer2D Drawer;
        private List<Boton> Botones;
        private int Separacion; //entre botones
        private int AlturaBotones;
        private CustomSprite Fondo;

        public int CantidadBotones => Botones.Count();

        public MenuPrincipal() {
            Drawer = new Drawer2D();
            Botones = new List<Boton>();
            Separacion = 50;
            AlturaBotones = 75;
            var fondo = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\fondo-menu.png", D3DDevice.Instance.Device);
            Fondo = new CustomSprite();
            Fondo.Bitmap = fondo;
            Fondo.Position = TGCVector2.Zero;
            Fondo.Scaling = new TGCVector2(100, 100);
        }

        public void AgregarBoton(string texto) {
            var posicionX = D3DDevice.Instance.Device.Viewport.Width / 2;
            var posicionY = CantidadBotones * (Separacion + AlturaBotones);
            Botones.Add(new Boton(texto, posicionX, posicionY));
        }

        public void Update(Puntero puntero) {
            Botones.ForEach(b => b.Update(puntero));
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.EndDrawSprite();
            Botones.ForEach(b => b.Render());            
        }

    }
}
