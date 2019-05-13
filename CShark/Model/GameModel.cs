using Microsoft.DirectX.DirectInput;
using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Mathematica;
using CShark.NPCs.Enemigos;
using CShark.Terreno;
using CShark.UI;
using CShark.UI.HUD;
using CShark.Jugador;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Textures;
using Microsoft.DirectX;

namespace CShark.Model
{
    public class GameModel : TgcExample {

        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir) {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        
        public static int DeviceWidth = D3DDevice.Instance.Device.Viewport.Width;
        public static int DeviceHeight = D3DDevice.Instance.Device.Viewport.Height;
        public static Point ScreenCenter = new Point(DeviceWidth / 2, DeviceHeight / 2);
        public Player Player;
        public Tiburon Tiburon;
        public GameManager GameManager;
        private PantallaMuerte PantallaMuerte;
        private Mapa Mapa => Mapa.Instancia;

        public override void Init() {
            Cursor.Hide();
            PantallaMuerte = new PantallaMuerte();            
            GameManager = new GameManager();

            //Te dejo todo esto comentado porque no estoy segura pero no te quiero borrar lo tuyo

            //var device = D3DDevice.Instance.Device;
            //float ancho = device.CreationParameters.FocusWindow.Width;
            //float alto = device.CreationParameters.FocusWindow.Height;
            //var anchoAltoMapa = 60000f;
            //var farDistance = FastMath.Sqrt(anchoAltoMapa * anchoAltoMapa * 2);
            //var transformacion= Matrix.PerspectiveFovLH(FastMath.QUARTER_PI, ancho / alto, 1f, farDistance);
            //device.Transform.Projection = transformacion;
            Start();
        }

        public override void Update() {

            PreUpdate();

            if (Input.keyPressed(Key.Escape) || Input.keyPressed(Key.I))
            {
                if (Player.EstaVivo)
                {
                    var tipomenu = Input.keyPressed(Key.Escape) ? TipoMenu.Principal : TipoMenu.Inventario;
                    CambiarMenu(tipomenu);
                    GameManager.SwitchMenu(this);
                }
                else
                {
                    Start();
                }
            }

            else
            {
                Mapa.Update(ElapsedTime, this);
                Player.Update(this);
                GameManager.Update(this);
            }
            
            PostUpdate();

        }

        private void Start() {
            var posInicial = GameManager.SpawnPlayer;
            Player = new Player(posInicial, 500, 1000, Input);
            Camara = Player.CamaraInterna;
            D3DDevice.Instance.Device.Transform.Projection = TGCMatrix.PerspectiveFovLH(45, D3DDevice.Instance.AspectRatio, D3DDevice.Instance.ZNearPlaneDistance, D3DDevice.Instance.ZFarPlaneDistance * 40f);
            CambiarMenu(TipoMenu.Guia);
            GameManager.SwitchMenu(this);
        }

        public override void Render() {

            PreRender();

            GameManager.Render(this);
            Mapa.Render(Player);

            if (Player.EstaVivo)
                Player.Render();
            else
                PantallaMuerte.Render();

            PostRender();

        }

        public override void Dispose() {
            GameManager.Dispose();
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            GameManager.CambiarMenu(tipoMenu);
        }

        public void Salir() {
            Environment.Exit(0);
        }

    }
}