using CShark.Managers;
using CShark.Model;
using CShark.Terreno;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using TGC.Core.Textures;
using Font = System.Drawing.Font;

namespace CShark.UI
{
    public class LoadingScreen
    {
        private int Progreso = 0;
        private int AnchoBarra => Barra.Bitmap.ImageInformation.Width;
        private int AltoBarra => Barra.Bitmap.ImageInformation.Height;
        private TgcText2D Texto;
        private CustomSprite Fondo;
        private CustomSprite Barra;
        private CustomSprite Logo;
        private Drawer2D Drawer;
        public bool Cargado = false;

        private int Operaciones;

        public LoadingScreen(int operaciones) {
            Operaciones = operaciones;
            Drawer = new Drawer2D();
            Fondo = new CustomSprite();
            Barra = new CustomSprite();
            Logo = new CustomSprite();
            Texto = new TgcText2D();
            Initialize();
        }

        private void Initialize() {
            var media = Game.Default.MediaDirectory;
            var viewport = D3DDevice.Instance.Device.Viewport;
            Fondo.Bitmap = new CustomBitmap(media + @"Menu\Loading\fondo.png", D3DDevice.Instance.Device);
            Fondo.Position = TGCVector2.Zero;
            Fondo.Scaling = EscalarFondo(Fondo.Bitmap, viewport);
            Barra.Bitmap = ObtenerBitmap(viewport.Width);
            Barra.Position = PosicionarBarra(viewport);
            Barra.Scaling = EscalarBarra(Barra.Bitmap);
            Logo.Bitmap = new CustomBitmap(media + @"Menu\Loading\logo.png", D3DDevice.Instance.Device);
            Logo.Position = TGCVector2.Zero;
            Logo.Scaling = EscalarFondo(Logo.Bitmap, viewport);
            Texto.Color = Color.White;
            Texto.Size = new Size(viewport.Width, 75);
            Texto.Align = TgcText2D.TextAlign.CENTER;
            var posT = Barra.Position - new TGCVector2(0, 56f);
            Texto.Position = new Point((int)posT.X, (int)posT.Y);
            Texto.changeFont(new Font("Arial", 25f, FontStyle.Regular));
        }

        private TGCVector2 EscalarBarra(CustomBitmap bitmap) {
            return new TGCVector2((float)AnchoBarra / bitmap.Width, (float)AltoBarra / bitmap.Height);
        }

        private TGCVector2 PosicionarBarra(Viewport viewport) {
            var separacionCostado = viewport.Width / 2 - AnchoBarra / 2;
            return new TGCVector2(separacionCostado, viewport.Height - separacionCostado - AltoBarra);
        }

        private TGCVector2 EscalarFondo(CustomBitmap bitmap, Viewport viewport) {
            return new TGCVector2((float)viewport.Width / bitmap.Width, (float)viewport.Height / bitmap.Height);
        }

        private CustomBitmap ObtenerBitmap(int anchoPantalla) {
            try
            {
                string archivo = "barra" + anchoPantalla + ".png";
                return new CustomBitmap(Game.Default.MediaDirectory + @"Menu\Loading\" + archivo, D3DDevice.Instance.Device);
            }
            catch (Exception e)
            {
                return new CustomBitmap(Game.Default.MediaDirectory + @"Menu\Loading\barra1920.png", D3DDevice.Instance.Device);
            }
        }

        public void Render() {

            while (!Cargado) {
                PreRender();
                Drawer.BeginDrawSprite();
                Drawer.DrawSprite(Fondo);
                Drawer.DrawSprite(Barra);
                Drawer.DrawSprite(Logo);
                Drawer.EndDrawSprite();
                Texto.render();
                PostRender();
            }
        }

        private void PreRender() {
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            D3DDevice.Instance.Device.BeginScene();
            TexturesManager.Instance.clearAll();
        }

        private void PostRender() {
            D3DDevice.Instance.Device.EndScene();
            D3DDevice.Instance.Device.Present();
        }

        public void Finalizar() {
            Cargado = true;
        }

        public void Progresar(string texto) {
            if (!Cargado)
            {
                Texto.Text = texto;
                Progreso++;
                var ancho = (float)Progreso / Operaciones * Barra.Bitmap.Width;
                Barra.SrcRect = new Rectangle(0, 0, (int)ancho, Barra.Bitmap.Height);
            }
        }

    }
}
