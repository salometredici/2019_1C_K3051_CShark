using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Terrain;
using TGC.Core.Textures;
using TGC.Group.Menu;

namespace TGC.Group.Model
{
    public class GameModel : TgcExample
    {
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir) {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        private Point mouseCenter;

        public bool MenuAbierto { get; set; }

        private TgcFpsCamera camaraInterna;
        private TgcSimpleTerrain terrain;
        private TgcScene scene;
        private Pez nemo;

        private MenuPrincipal menu;

        public override void Init() {
            var d3dDevice = D3DDevice.Instance.Device;
            mouseCenter = new Point(D3DDevice.Instance.Device.Viewport.Width / 2, D3DDevice.Instance.Device.Viewport.Height / 2);

            menu = new MenuPrincipal();
            menu.AgregarBoton("Opciones");
            menu.AgregarBoton("ASDASDASD");
            menu.AgregarBoton("ASDASDASD");
            menu.AgregarBoton("ASDASDASD");


            var loader = new TgcSceneLoader();
            terrain = new TgcSimpleTerrain();
            terrain.loadHeightmap(MediaDir + "Heightmaps\\heightmap.jpg", 50, 1.3f, new TGCVector3(0, -200, 0));
            terrain.loadTexture(MediaDir + "Textures\\arena.jpg");
            scene = loader.loadSceneFromFile(MediaDir + "prueba-TgcScene.xml");
            nemo = new Pez(scene.Meshes[4], 2f, 50f);
            camaraInterna = new TgcFpsCamera(new TGCVector3(-210, 218, -665), 500f, 0.1f, Input);
            Camara = camaraInterna;
        }

        public override void Update() {
            PreUpdate();

            if (Input.keyPressed(Key.Escape))
            {
                MenuAbierto = !MenuAbierto;
                camaraInterna.Lock();
            }

            if (MenuAbierto)
            {
                menu.Update(Input);
                Cursor.Show();
            }
            else
            {
                nemo.Moverse(ElapsedTime);
                Cursor.Hide();
                Cursor.Position = mouseCenter;
            }

            PostUpdate();
        }

        public override void Render() {
            PreRender();

            DrawText.drawText("Camara: " + TGCVector3.PrintVector3(Camara.Position), 5, 20, Color.Red);
            DrawText.drawText("Con ESC abris el intento de menu", 5, 40, Color.Red);
            terrain.Render();
            scene.RenderAll();

            if (MenuAbierto)
                menu.Render();

            PostRender();
        }

        public override void Dispose() {
            scene.DisposeAll();
            terrain.Dispose();
        }
    }
}