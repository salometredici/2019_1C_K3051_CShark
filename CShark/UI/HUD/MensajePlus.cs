using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.Text;
using Font = System.Drawing.Font;

namespace CShark.UI.HUD
{
    public class MensajePlus : IDisposable
    {
        private CustomSprite Sprite;
        private TgcText2D Texto;
        private float TiempoMostrado;
        private float AnchoPantalla;
        private MensajesContainer Container;

        private readonly float altoContainer = 100f;
        private readonly float anchoImagen = 60f;
        private readonly float altoImagen = 60f;

        public MensajePlus(string bitmap, string tipo, MensajesContainer container) {
            var path = Game.Default.MediaDirectory + @"Items\" + bitmap;
            var device = D3DDevice.Instance.Device;
            AnchoPantalla = device.Viewport.Width;
            Sprite = new CustomSprite();
            Sprite.Bitmap = new CustomBitmap(path, device);
            Sprite.Scaling = new TGCVector2(anchoImagen / Sprite.Bitmap.Width, altoImagen / Sprite.Bitmap.Height);
            Texto = new TgcText2D {
                Format = DrawTextFormat.VerticalCenter,
                Color = Color.White,
                Size = new Size(200, 100),
                Text = "+ 1 " + tipo
            };
            Texto.changeFont(new Font("Arial", 22f, FontStyle.Bold));
            TiempoMostrado = 0;
            Container = container;
            Activar();
        }
               
        public void Render(Drawer2D drawer) {
            drawer.DrawSprite(Sprite);
            Texto.render();
        }        

        public void Update(float elapsedTime) {
            TiempoMostrado += elapsedTime;
            if (TiempoMostrado > 2f) {
                Texto.Position = new Point(Texto.Position.X + 1, Texto.Position.Y);
                Sprite.Position += new TGCVector2(1, 0);
            }
            if (Sprite.Position.X > AnchoPantalla) {
                Container.Desactivar();
            }
        }

        public void Activar() {
            var device = D3DDevice.Instance.Device;
            var posicion = new TGCVector2(device.Viewport.Width - 300, device.Viewport.Height - 100);
            var spritePos = posicion + new TGCVector2(0, altoContainer / 2f - altoImagen / 2f);
            Sprite.Position = spritePos;
            Texto.Position = new Point((int)posicion.X + 100, (int)posicion.Y);
            TiempoMostrado = 0;
        }

        public void Dispose() {
            Sprite.Dispose();
            Texto.Dispose();
        }
    }
}
