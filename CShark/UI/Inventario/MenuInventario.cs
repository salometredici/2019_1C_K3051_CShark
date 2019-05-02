﻿using System;
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
        private List<Boton> BotonesItems;
        private List<CustomSprite> SpritesItems;
        private static string InventoryDir = Game.Default.InventoryDir;

        private string[,] GrillaRutas = new string[,]
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
            BotonesItems = new List<Boton>();
            SpritesItems = new List<CustomSprite>();
            SetGrillaItems();
            Separacion = 50;
            AlturaBotones = 75;
            SetItem(Title, InventoryDir + "inventory-title-menu.png", new TGCVector2(Title.Position.X, Title.Position.Y + DeviceHeight / 12), Title.Scaling);
            SetItem(Logo, InventoryDir + "woodboard1.png", new TGCVector2(DeviceWidth / 12 - 40, DeviceHeight / 12 + 30), new TGCVector2(0.68f, 0.575f));
            CrearInventario();
        }

        private void SetGrillaItems()
        {
            var Columna0PosX = Logo.Position.X - 40;
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
            AgregarBoton("Volver", j => j.CambiarMenu(TipoMenu.Principal));
            AgregarItems();
        }

        public void AgregarItems()
        {
            for (int i = 0;i<3;i++)
            {
                for(int j = 0; j<2;j++)
                {
                    var item = new CustomSprite();
                    var posicionX = GrillaItems[i, j].X;
                    var posicionY = GrillaItems[i, j].Y;
                    SetItem(item, GrillaRutas[i, j], new TGCVector2(posicionX, posicionY), new TGCVector2(0.625f,0.625f));
                    SpritesItems.Add(item);
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
            Botones.ForEach(b => b.Update(juego));
        }

        public override void Render()
        {
            base.Render();
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Title);
            for(int i = 0; i<SpritesItems.Count();i++)
            {
                Drawer.DrawSprite(SpritesItems[i]);
            }
            Drawer.EndDrawSprite();
            Botones.ForEach(b => b.Render());
        }
    }
}
