using Microsoft.DirectX.DirectInput;
using System;
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
using TGC.Group.Managers;
using TGC.Group.NPCs.Peces;
using TGC.Group.Terreno;
using TGC.Group.UI;
using TGC.Group.UI.HUD;
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
        private Variable<float> VelocidadMovimiento;
        private Variable<float> VelocidadRotacion;
        private Variable<bool> ModoDios;
        private Variable<bool> Niebla;
        private Variable<bool> PostProcesadoCasco;
        private Variable<bool> MotionBlur;

        private Point mouseCenter;
        private Point CentroPantalla;

        public bool MenuAbierto { get; set; }

        private Puntero puntero;
        public TgcFpsCamera camaraInterna;
        private TgcSimpleTerrain terrain;
        private Jugador jugador;
        private TgcMesh tiburon;
        private Superficie superficie;

        private PezManager PezManager;


        private MenuPrincipal MenuPrincipal;
        private MenuOpciones MenuOpciones;
        private MenuVariables MenuVariables;
        private PantallaMuerte PantallaMuerte;

        private UI.Menu MenuSeleccionado;

                                  
        public override void Init() {

            Cursor.Hide();

            CargarVariables();
            CargarModelos();
            PantallaMuerte = new PantallaMuerte();
            var d3dDevice = D3DDevice.Instance.Device;
            mouseCenter = new Point(D3DDevice.Instance.Device.Viewport.Width / 2, D3DDevice.Instance.Device.Viewport.Height / 2);
            puntero = new Puntero();

            Start();
        }

        public override void Update() {
            PreUpdate();
            
            if (Input.keyPressed(Key.Escape))
            {
                camaraInterna.Lock();
                if (jugador.EstaVivo)
                {
                    CambiarMenu(TipoMenu.Principal);
                    MenuAbierto = !MenuAbierto;
                }
                else Start();
            }
            if (MenuAbierto)
            {
                MenuSeleccionado.Update(this);
                puntero.Update();
            }
            else
            {
                superficie.Update(ElapsedTime);
                jugador.Update(camaraInterna);
                PezManager.Update(ElapsedTime);
                Cursor.Position = mouseCenter;
            }

            
            PostUpdate();
        }

        private void Start() {
            var posicionInicial = new TGCVector3(-210, 218, -665);
            camaraInterna = new TgcFpsCamera(posicionInicial, VelocidadMovimiento, VelocidadRotacion, Input);
            Camara = camaraInterna;
            jugador = new Jugador(TGCVector3.Empty, 500, 100000);
        }

        public override void Render() {
            PreRender();

            DrawText.drawText("Camara: " + TGCVector3.PrintVector3(Camara.Position), 5, 20, Color.Red);
            DrawText.drawText("Con ESC abris el intento de menu", 5, 40, Color.Red);
            terrain.Render();
            PezManager.Render();
            tiburon.Render();
            superficie.Render();

            if (jugador.EstaVivo)
                jugador.Render();
            else
                PantallaMuerte.Render();

            if (MenuAbierto)
            {
                MenuSeleccionado.Render();
                puntero.Render();
            }            

            PostRender();
        }

        public override void Dispose() {
            terrain.Dispose();
            tiburon.Dispose();
            superficie.Dispose();
        }

        private void CargarVariables() {
            var viewport = D3DDevice.Instance.Device.Viewport;
            CentroPantalla = new Point(viewport.Width / 2, viewport.Height / 2);
            MenuPrincipal = new MenuPrincipal();
            MenuOpciones = new MenuOpciones();
            MenuVariables = new MenuVariables();
            MenuSeleccionado = MenuPrincipal;

            VelocidadMovimiento = new Variable<float>("Velocidad de movimiento", 500f);
            VelocidadRotacion = new Variable<float>("Velocidad de rotación", 0.1f);
            ModoDios = new Variable<bool>("Modo Dios", true);
            Niebla = new Variable<bool>("Niebla", false);
            PostProcesadoCasco = new Variable<bool>("Post Procesado (Casco)", false);
            MotionBlur = new Variable<bool>("Motion Blur", false);

            MenuPrincipal.AgregarBoton("Opciones", j => j.CambiarMenu(TipoMenu.Opciones));
            MenuPrincipal.AgregarBoton("Variables", j => j.CambiarMenu(TipoMenu.Variables));
            MenuPrincipal.AgregarBoton("Cheats", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Salir", j => j.Salir());

            MenuOpciones.AgregarCheckbox(new Checkbox(ModoDios, CentroPantalla.X, 100));
            MenuOpciones.AgregarCheckbox(new Checkbox(Niebla, CentroPantalla.X, 200));
            MenuOpciones.AgregarCheckbox(new Checkbox(PostProcesadoCasco, CentroPantalla.X, 300));
            MenuOpciones.AgregarCheckbox(new Checkbox(MotionBlur, CentroPantalla.X, 400));

            MenuVariables.AgregarSlider(new Slider(VelocidadRotacion, 0.05f, 0.2f, CentroPantalla.X, 100));
            MenuVariables.AgregarSlider(new Slider(VelocidadMovimiento, 200, 1000, CentroPantalla.X, 200));
        }

        private void CargarModelos() {
            var loader = new TgcSceneLoader();
            terrain = new TgcSimpleTerrain();
            terrain.loadHeightmap(MediaDir + "Heightmaps\\heightmap.jpg", 50, 1.3f, new TGCVector3(0, -200, 0));
            terrain.loadTexture(MediaDir + "Textures\\arena.jpg");

            PezManager = new PezManager();
            PezManager.CargarPez(new PezPayaso(0, 0, 0));
            PezManager.CargarPez(new PezAzul(100, 0, 100));
            PezManager.CargarPez(new PezBetta(0, 0, 300));
            //PezManager.CargarPez(new PezCoral(200, 100, 0)); esta basura rompe :(
            PezManager.CargarPez(new PezTropical(1, 0, 100, 0));
            PezManager.CargarPez(new PezTropical(2, 0, 200, 0));
            PezManager.CargarPez(new PezTropical(3, 0, 300, 0));
            PezManager.CargarPez(new PezTropical(4, 0, 400, 0));
            PezManager.CargarPez(new PezTropical(5, 0, 500, 0));
            PezManager.CargarPez(new PezTropical(6, 0, 600, 0));

            superficie = new Superficie();
                       
            tiburon = loader.loadSceneFromFile(MediaDir + "tiburoncin-TgcScene.xml").Meshes[0];
            tiburon.Position += new TGCVector3(2000, 500, 500);
            tiburon.Scale = new TGCVector3(200, 200, 200);
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
                case TipoMenu.Variables:
                    return MenuVariables;
            }
            return null;
        }

        public void Salir() {
            Environment.Exit(0);
        }

    }
}