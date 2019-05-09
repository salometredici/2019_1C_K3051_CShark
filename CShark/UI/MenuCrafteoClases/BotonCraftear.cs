using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;

namespace CShark.UI
{
    public class BotonCraftear
    {
        private CustomSprite Sprite;

        public BotonCraftear(TGCVector2 posicionMenu, float ancho, float alto) {
            Sprite = new CustomSprite();
            Sprite.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + @"MenuCrafteo\craft.png", D3DDevice.Instance.Device);
            float anchoReal = Sprite.Bitmap.ImageInformation.Width;
            float altoReal = Sprite.Bitmap.ImageInformation.Height;
            Sprite.Scaling = new TGCVector2(anchoReal / Sprite.Bitmap.Width, altoReal / Sprite.Bitmap.Height);
            Sprite.Position = posicionMenu + new TGCVector2(ancho - anchoReal - 10, alto - altoReal - 10);
        }

        public void Update() {
            //asd
        }

        public void Render(Drawer2D drawer) {
            drawer.DrawSprite(Sprite);
        }
    }
}
