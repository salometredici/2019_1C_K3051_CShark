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
        private List<BotonInventario> BotonesItems;
        private static string InventoryDir = Game.Default.InventoryDir;

        private readonly string[,] GrillaRutas = new string[,]
        {
           { InventoryDir + "wumpa-fruit-inventory.png", InventoryDir + "coral-inventory.png" },
           { InventoryDir + "batteries-inventory.png", InventoryDir + "medkit-inventory.png" },
           { InventoryDir + "seashell-inventory.png", InventoryDir + "oxygen-inventory.png" }
        };

        private TGCVector2[,] GrillaItems;

        public int CantidadBotones => Botones.Count();

        public MenuInventario() : base()
        {
            Drawer = new Drawer2D();
            Botones = new List<Boton>();
            BotonesItems = new List<BotonInventario>();
            SetGrillaItems();
            Separacion = 50;
            AlturaBotones = 75;
            SetItem(Title, InventoryDir + "inventory-title-menu.png", new TGCVector2(Title.Position.X, Title.Position.Y + DeviceHeight / 12), Title.Scaling);
            SetItem(Logo, InventoryDir + "woodboard1.png", new TGCVector2(DeviceWidth / 12 - 30, DeviceHeight / 12 + 30), new TGCVector2(0.68f, 0.575f));
            CrearInventario();
        }

        private void SetGrillaItems()
        {
            var Columna0PosX = Logo.Position.X - 70;
            var Columna1PosX = Columna0PosX + 275 + 70;
            var Fila0PosY = Logo.Position.Y + 25;
            GrillaItems = new TGCVector2[,] {
                { new TGCVector2(Columna0PosX, Fila0PosY), new TGCVector2(Columna1PosX, Fila0PosY) },
                { new TGCVector2(Columna0PosX, Fila0PosY + 185), new TGCVector2(Columna1PosX, Fila0PosY + 185) },
                { new TGCVector2(Columna0PosX, Fila0PosY + 370), new TGCVector2(Columna1PosX, Fila0PosY + 370) }
            };
        }

        public void CrearInventario()
        {
            AgregarBoton("Craft", j => j.CambiarMenu(TipoMenu.Principal));
            AgregarBoton("Combos de crafteo", j => j.CambiarMenu(TipoMenu.Guia));
            AgregarBoton("Volver", j => j.CambiarMenu(TipoMenu.Principal));
            AgregarBotonesInventario();
        }


        public void AgregarBotonesInventario()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var item = new CustomSprite();
                    var posicionX = (int)GrillaItems[i, j].X;
                    var posicionY = (int)GrillaItems[i, j].Y;
                    SetItem(item, GrillaRutas[i, j], new TGCVector2(posicionX, posicionY), new TGCVector2(0.625f, 0.625f));
                    BotonesItems.Add(new BotonInventario(item, "--", posicionX, posicionY, k => k.CambiarMenu(TipoMenu.Principal)));
                }
            }
        }

        public void AgregarBoton(string texto, Action<GameModel> accion)
        {
            var posicionX = (int)RightMenuXPos_X;
            var posicionY = CantidadBotones > 0 ? Botones.Last().Posicion.Y + Separacion : (int)RightMenuPos_Y + 100;
            Botones.Add(new Boton(texto, posicionX, posicionY, accion));
        }

        public override void Update(GameModel juego)
        {
            BotonesItems.ForEach(b => b.Update(juego));
            Botones.ForEach(b => b.Update(juego));
        }

        public override void Render()
        {
            base.Render();
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Title);
            Drawer.EndDrawSprite();
            BotonesItems.ForEach(b => b.Render());
            Botones.ForEach(b => b.Render());
        }
    }
}
