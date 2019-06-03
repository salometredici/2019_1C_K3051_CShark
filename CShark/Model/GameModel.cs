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
using CShark.EfectosLuces;
using TGC.Core.Shaders;
using CShark.Items;

namespace CShark.Model
{
    public class GameModel : TgcExample
    {

        public static int DeviceWidth = D3DDevice.Instance.Device.Viewport.Width;
        public static int DeviceHeight = D3DDevice.Instance.Device.Viewport.Height;
        public static Point ScreenCenter = new Point(DeviceWidth / 2, DeviceHeight / 2);
        public Player Player;
        public GameManager GameManager;
        private PantallaMuerte PantallaMuerte;
        private Mapa Mapa => Mapa.Instancia;
        private Casco Casco;
        private bool RenderCasco = false;

        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        public override void Init()
        {
            Cursor.Hide();
            PantallaMuerte = new PantallaMuerte();
            GameManager = new GameManager();
            Casco = new Casco();
            Start();
        }

        private void Start()
        {
            var posInicial = GameManager.SpawnPlayer;
            Player = new Player(posInicial, 500, 1000, Input);
            Camara = Player.CamaraInterna;
            CambiarMenu(TipoMenu.Guia);
            GameManager.SwitchMenu(this);
        }

        public override void Update()
        {

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

        public override void Render()
        {
            RenderCasco = Configuracion.Instancia.PostProcesadoCasco.Valor && !Player.onPause;
            BackgroundColor = Efectos.Instancia.colorNiebla;
            ClearTextures();

            PreRender();
            
            if (RenderCasco)
            {
                Casco.RenderBeforeScene();
            }

            GameManager.Render(this);
            Mapa.Render(this);

            if (Player.EstaVivo)
                Player.Render();
            else
                PantallaMuerte.Render();

            if (RenderCasco)
            {
                Casco.RenderAfterScene();
            }

            PostRender();

        }

        public override void Dispose()
        {
            GameManager.Dispose();
            Casco.Dispose();
            Mapa.Instancia.Dispose();
            GameManager.Dispose();
        }

        public void CambiarMenu(TipoMenu tipoMenu)
        {
            GameManager.CambiarMenu(tipoMenu);
        }

        public void UsarItem(ERecolectable item)
        {
            Player.Usar(item);
        }

        public void Salir()
        {
            Environment.Exit(0);
        }

    }
}