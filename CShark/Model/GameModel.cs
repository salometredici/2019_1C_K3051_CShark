using Microsoft.DirectX.DirectInput;
using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core.Terrain;
using CShark.Managers;
using CShark.NPCs.Enemigos;
using CShark.NPCs.Peces;
using CShark.Terreno;
using CShark.UI;
using CShark.UI.HUD;
using CShark.Variables;
using CShark.Jugador;
using CShark.Utils;
using System.Threading.Tasks;
using System.Threading;

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
        private GameManager GameManager;
        private PantallaMuerte PantallaMuerte;
        private Mapa Mapa => Mapa.Instancia;

        public override void Init() {
            Cursor.Hide();
            PantallaMuerte = new PantallaMuerte();            
            GameManager = new GameManager(); 
            Start();
        }

        public override void Update() {

            PreUpdate();

            if (Input.keyPressed(Key.Escape) || Input.keyPressed(Key.I))
            {
                Player.Lock();
                if (Player.EstaVivo)
                {
                    Player.onPause = !Player.onPause;
                    var tipomenu = Input.keyPressed(Key.Escape) ? TipoMenu.Principal : TipoMenu.Inventario;
                    CambiarMenu(tipomenu);
                    GameManager.SwitchMenu();
                }
                else
                {
                    Start();
                }
            }

            else
            {
                Mapa.Update(ElapsedTime, Player);
                Player.Update(this);
                GameManager.Update(this);
            }
            
            PostUpdate();

        }

        private void Start() {
            var posInicial = new TGCVector3(1500f, 3050f, 0);
            Player = new Player(posInicial, 500, 1000, Input);
            Camara = Player.CamaraInterna;
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