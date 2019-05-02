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

namespace CShark.UI.Inventario
{
    class MenuInventario : Menu
    {
        private Drawer2D Drawer;
        private List<Boton> Botones;
        private int Separacion;
        private int AlturaBotones;

        public int CantidadBotones => Botones.Count();

        public MenuInventario() : base()
        {
            Drawer = new Drawer2D();
            Botones = new List<Boton>();
            Separacion = 50;
            AlturaBotones = 75;
            SetItem(Title, "Menu\\Inventario\\inventory-title-menu.png", new TGCVector2(Title.Position.X, Title.Position.Y + DeviceHeight / 12), Title.Scaling);
            SetItem(Logo, "Menu\\Inventario\\woodboard1.png", new TGCVector2(DeviceWidth/12,DeviceHeight/12), new TGCVector2(0.65f,0.625f));
        }

        public void AgregarBoton(string texto, Action<GameModel> accion)
        {
            var posicionX = DeviceWidth / 2 + DeviceWidth / 4;
            var posicionY = CantidadBotones > 0 ? Botones.Last().Posicion.Y + Separacion : DeviceHeight / 4 + 100;
            Botones.Add(new Boton(texto, posicionX, posicionY, accion));
        }

        public override void Update(GameModel juego)
        {
            Botones.ForEach(b => b.Update(juego));
        }

        public override void Render()
        {
            base.Render();
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Title);
            Drawer.EndDrawSprite();
            Botones.ForEach(b => b.Render());
        }
    }
}
