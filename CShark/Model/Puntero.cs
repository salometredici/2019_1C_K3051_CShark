using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using CShark.Utils;

namespace CShark.Model
{
    public class Puntero
    {
        private CustomSprite Sprite;
        private Drawer2D Drawer;

        public TGCVector2 Posicion { get; set; }

        private readonly float Ancho = 32;
        private readonly float Alto = 32;

        public Puntero() {
            Drawer = new Drawer2D();
            var bitmap = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\puntero.png", D3DDevice.Instance.Device);
            Sprite = new CustomSprite();
            Sprite.Bitmap = bitmap;
            Sprite.Scaling = new TGCVector2(Ancho / bitmap.Width, Alto / bitmap.Height);
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Sprite);
            Drawer.EndDrawSprite();
        }

        public void Update() {
            Posicion = new TGCVector2(Cursor.Position.X, Cursor.Position.Y);
            Sprite.Position = Posicion;
        }


    }
}
