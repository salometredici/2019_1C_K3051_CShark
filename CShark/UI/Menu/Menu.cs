using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using CShark.Model;
using static CShark.Model.GameModel;
using CShark.Utils;

namespace CShark.UI
{
    public abstract class Menu : IDisposable
    {
        private readonly CustomSprite Fondo;
        protected CustomSprite Logo; // porque se va a poder modificar desde el inventario
        protected CustomSprite Title;

        protected readonly float RightMenuXPos_X = DeviceWidth / 2 + DeviceWidth / 4;
        protected readonly float RightMenuPos_Y = DeviceHeight / 4;

        private Drawer2D Drawer;

        public Menu() {
            Drawer = new Drawer2D();
            Fondo = new CustomSprite();
            Title = new CustomSprite();
            Logo = new CustomSprite();
            SetItem(Fondo, Game.Default.MediaDirectory + "\\Menu\\fondo-menu.png", TGCVector2.Zero, new TGCVector2(100, 100));
            SetItem(Title, Game.Default.MediaDirectory +"\\Menu\\menu-title.png", new TGCVector2(RightMenuXPos_X - 180, RightMenuPos_Y - DeviceHeight/12), new TGCVector2(0.7f, 0.5f));
            SetItem(Logo, Game.Default.MediaDirectory + "\\Menu\\logo.png", new TGCVector2(DeviceWidth / 8, DeviceHeight / 8), new TGCVector2(1.25f, 1.25f));
        }

        public void SetItem(CustomSprite item, string route, TGCVector2 position, TGCVector2 scaling)
        {
            item.Bitmap = new CustomBitmap(route, D3DDevice.Instance.Device);
            item.Position = position;
            item.Scaling = scaling;
        }

        public abstract void Update(GameModel juego);

        public virtual void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.DrawSprite(Title);
            Drawer.DrawSprite(Logo);
            Drawer.EndDrawSprite();
        }

        public void Dispose() {
            Fondo.Dispose();
            Logo.Dispose();
            Title.Dispose();
        }
    }
}
