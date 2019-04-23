using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using CShark.Model;
using CShark.Utils;

namespace CShark.UI
{
    public abstract class Menu
    {
        private CustomSprite Fondo;
        private CustomSprite Logo;

        private Drawer2D Drawer;

        public Menu() {
            Drawer = new Drawer2D();
            Fondo = new CustomSprite();
            Logo = new CustomSprite();
            Fondo.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\fondo-menu.png", D3DDevice.Instance.Device);
            Fondo.Position = TGCVector2.Zero;
            Fondo.Scaling = new TGCVector2(100, 100);
            Logo.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\logo.png", D3DDevice.Instance.Device);
        }

        public abstract void Update(GameModel juego);

        public virtual void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.DrawSprite(Logo);
            Drawer.EndDrawSprite();
        }
    }
}
