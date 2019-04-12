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
using TGC.Group.UI;
using TGC.Group.Variables;

namespace TGC.Group.Model
{
    public class GameModel : TgcExample {
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir) {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        //CONFIGURACION
        private Variable VelocidadMovimiento;
        private Variable VelocidadRotacion;

        private Point mouseCenter;
        private Point CentroPantalla;

        public bool MenuAbierto { get; set; }

        private Puntero puntero;
        public TgcFpsCamera camaraInterna;
        private TgcSimpleTerrain terrain;
        private TgcScene scene;
        private Pez nemo;

        private MenuPrincipal MenuPrincipal;
        private MenuOpciones MenuOpciones;

        private UI.Menu MenuSeleccionado;

        public override void Init() {

            Cursor.Hide();

            CargarVariables();

            var d3dDevice = D3DDevice.Instance.Device;
            mouseCenter = new Point(D3DDevice.Instance.Device.Viewport.Width / 2, D3DDevice.Instance.Device.Viewport.Height / 2);

            puntero = new Puntero();




            var loader = new TgcSceneLoader();
            terrain = new TgcSimpleTerrain();
            terrain.loadHeightmap(MediaDir + "Heightmaps\\heightmap.jpg", 50, 1.3f, new TGCVector3(0, -200, 0));
            terrain.loadTexture(MediaDir + "Textures\\arena.jpg");
            scene = loader.loadSceneFromFile(MediaDir + "prueba-TgcScene.xml");
            nemo = new Pez(scene.Meshes[4], 2f, 50f);
            var posicionInicial = new TGCVector3(-210, 218, -665);
            camaraInterna = new TgcFpsCamera(posicionInicial, VelocidadMovimiento, VelocidadRotacion, Input);
            Camara = camaraInterna;
        }

        public override void Update() {
            PreUpdate();

            if (Input.keyPressed(Key.Escape))
            {
                CambiarMenu(TipoMenu.Principal);
                MenuAbierto = !MenuAbierto;
                camaraInterna.Lock();
            }

            if (MenuAbierto)
            {
                MenuSeleccionado.Update(this);
                puntero.Update();
            }
            else
            {
                nemo.Moverse(ElapsedTime);
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
            {
                MenuSeleccionado.Render();
                puntero.Render();
            }

            PostRender();
        }

        public override void Dispose() {
            scene.DisposeAll();
            terrain.Dispose();
        }

        private void CargarVariables() {
            var viewport = D3DDevice.Instance.Device.Viewport;
            CentroPantalla = new Point(viewport.Width / 2, viewport.Height / 2);
            MenuPrincipal = new MenuPrincipal();
            MenuOpciones = new MenuOpciones();
            MenuSeleccionado = MenuPrincipal;

            VelocidadMovimiento = new Variable("Velocidad de movimiento", 500f);
            VelocidadRotacion = new Variable("Velocidad de rotación", 0.1f);

            MenuPrincipal.AgregarBoton("Opciones", j => j.CambiarMenu(TipoMenu.Opciones));
            MenuPrincipal.AgregarBoton("Variables", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Cheats", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Salir", j => j.CambiarMenu(TipoMenu.Principal));

            MenuOpciones.AgregarSlider(new Slider(VelocidadRotacion, 0.05f, 0.2f, CentroPantalla.X, 100));
            MenuOpciones.AgregarSlider(new Slider(VelocidadMovimiento, 200, 1000, CentroPantalla.X, 200));
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            MenuSeleccionado = NuevoMenu(tipoMenu);
        }

        private UI.Menu NuevoMenu(TipoMenu tipo) {
            switch(tipo)
            {
                case TipoMenu.Principal:
                    return MenuPrincipal;
                case TipoMenu.Opciones:
                    return MenuOpciones;
                //case 
            }
            return null;
        }

    }
}