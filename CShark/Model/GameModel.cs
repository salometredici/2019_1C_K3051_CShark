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

namespace CShark.Model
{
    public class GameModel : TgcExample {

        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir) {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        
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

            if (Input.keyPressed(Key.Escape))
            {
                Player.Lock();
                if (Player.EstaVivo)
                {
                    Player.onPause = !Player.onPause;
                    CambiarMenu(TipoMenu.Principal);
                    GameManager.SwitchMenu();
                }
                else
                {
                    Start();
                }
            }

            else
            {
                Mapa.Update();
                Player.Update(this);
                GameManager.Update(this);
            }
            
            PostUpdate();

        }

        private void Start() {
            var posInicial = new TGCVector3(0, 500f, 1000f);
            Player = new Player(posInicial, 500, 1000, Input);
            Camara = Player.CamaraInterna;
        }

        public override void Render() {

            PreRender();

            GameManager.Render(this);
            Mapa.Render(Player.Posicion);

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