using CShark.UI.HUD;
using CShark.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;

namespace CShark.Jugador
{
    public class HUD
    {
        private Drawer2D Drawer;
        public BarraVida BarraVida;
        public BarraOxigeno BarraOxigeno;
        private CustomSprite Crosshair;        

        public HUD(float vida, float oxigeno) {
            int alturaTotal = D3DDevice.Instance.Device.Viewport.Height;
            BarraVida = new BarraVida(new TGCVector2(15, alturaTotal - 140), vida);
            BarraOxigeno = new BarraOxigeno(new TGCVector2(15, alturaTotal - 75), oxigeno);
            Crosshair = CargarCrosshair();
            Drawer = new Drawer2D();
        }

        public void Update(float vida, float oxigeno) {
            BarraVida.Update(vida);
            BarraOxigeno.Update(oxigeno);
        }

        public void Render() {
            BarraVida.Render();
            BarraOxigeno.Render();
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Crosshair);
            Drawer.EndDrawSprite();
        }

        private CustomSprite CargarCrosshair() {
            var sprite = new CustomSprite();
            var device = D3DDevice.Instance.Device;
            sprite.Bitmap = new CustomBitmap(Game.Default.MediaDirectory + @"Textures\crosshair.png", device);
            float ancho = sprite.Bitmap.ImageInformation.Width;
            float alto = sprite.Bitmap.ImageInformation.Height;
            sprite.Position = new TGCVector2(device.Viewport.Width / 2f, device.Viewport.Height / 2f);
            sprite.Scaling = new TGCVector2(ancho / sprite.Bitmap.Width, alto / sprite.Bitmap.Height);
            return sprite;
        }
    }
}
