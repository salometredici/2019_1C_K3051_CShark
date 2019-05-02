using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Text;
using System.Drawing;
using CShark.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using System.Windows.Forms;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;
using CShark.Model;
using Font = System.Drawing.Font;

namespace CShark.UI
{
    public class BotonInventario : Boton
    {
        public new Point Posicion;
        private readonly string InventoryDir = Game.Default.InventoryDir;

        public BotonInventario(CustomSprite item, string texto, int x, int y, Action<GameModel> action) : base(item,texto,x,y,action) {
            CargarTexto(texto);
            Posicion = new Point(x, y);
        }

        public override void CargarTexto(string texto) {
            Texto = new TgcText2D
            {
                Color = Color.White,
                Text = texto,
                Size = new Size(Ancho, Alto),
                Format = DrawTextFormat.Center,
                Position = Posicion
            };
            Texto.changeFont(new Font("Arial", 25, FontStyle.Bold, GraphicsUnit.Pixel));
            Texto.Position = new Point(Posicion.X + 55, Posicion.Y + 55);
        }

        public bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }
    }
}
