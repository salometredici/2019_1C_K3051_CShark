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
using TGC.Core.Interpolation;
using Effect = Microsoft.DirectX.Direct3D.Effect;
using Device = Microsoft.DirectX.Direct3D.Device;
using TGC.Core.Sound;
using CShark.Utils;
using static CShark.Utils.EffectsPlayer;
using System.Collections.Generic;
using TGC.Core.SkeletalAnimation;
using TGC.Core.SceneLoader;
using TGC.Core.Geometry;

namespace CShark.Model
{
    public class GameModel : TgcExample
    {
        #region Members
        public static int DeviceWidth = D3DDevice.Instance.Device.Viewport.Width;
        public static int DeviceHeight = D3DDevice.Instance.Device.Viewport.Height;
        public static Point ScreenCenter = new Point(DeviceWidth / 2, DeviceHeight / 2);
        public Player Player;
        public GameManager GameManager;
        public EffectsPlayer EffectsPlayer;
        private PantallaMuerte PantallaMuerte;
        private Mapa Mapa => Mapa.Instancia;
        private bool RenderCasco = true;
        public static Device Device => D3DDevice.Instance.Device;
        private readonly int cant_pasadas = 3;
        private Effect effect;
        private Surface g_pDepthStencil; // Depth-stencil buffer
        private Texture g_pRenderTarget, g_pGlowMap, g_pRenderTarget4, g_pRenderTarget4Aux;
        private VertexBuffer g_pVBV3D;
        private TgcTexture cascoTexture;
        Surface pSurf, pOldRT, pOldDS;
        #endregion

        #region Constructors
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir) {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        #endregion

        #region Init
        public override void Init() {

            Cursor.Hide();
            PantallaMuerte = new PantallaMuerte();
            GameManager = new GameManager();

            string compilationErrors;
            effect = Effect.FromFile(D3DDevice.Instance.Device, ShadersDir + "PostProcesado.fx", null, null, ShaderFlags.PreferFlowControl, null, out compilationErrors);
            if (effect == null) {
                throw new Exception("Error al cargar shader. Errores: " + compilationErrors);
            }
            effect.Technique = "DefaultTechnique";
            g_pDepthStencil = Device.CreateDepthStencilSurface(Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight,
                DepthFormat.D24S8, MultiSampleType.None, 0, true);
            g_pRenderTarget = new Texture(Device, Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);
            g_pGlowMap = new Texture(Device, Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);
            g_pRenderTarget4 = new Texture(Device, Device.PresentationParameters.BackBufferWidth / 4, Device.PresentationParameters.BackBufferHeight / 4, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);
            g_pRenderTarget4Aux = new Texture(Device, Device.PresentationParameters.BackBufferWidth / 4, Device.PresentationParameters.BackBufferHeight / 4, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);
            effect.SetValue("g_RenderTarget", g_pRenderTarget);
            effect.SetValue("screen_dx", Device.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", Device.PresentationParameters.BackBufferHeight);

            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            g_pVBV3D = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            g_pVBV3D.SetData(vertices, 0, LockFlags.None);
            cascoTexture = TgcTexture.createTexture(Device, MediaDir + @"Textures\casco.png");

            Start();
        }
        #endregion

        public override void Render() {
            RenderCasco = Configuracion.Instancia.PostProcesadoCasco.Valor && !Player.onPause;
            BackgroundColor = Efectos.Instancia.colorNiebla;

            ClearTextures();

            this.RenderEscenaTarget();
            this.RenderGlowMap();
            this.BlurGlowMap();
            this.RenderEscena();
            this.RenderPlayer();

            Device.BeginScene();
            RenderFPS();
            RenderAxis();
            HUD.Instancia.Render();
            Device.EndScene();
            Device.Present();
        }

        public override void Update() {

            PreUpdate();

            if (Input.keyPressed(Key.Escape) || Input.keyPressed(Key.I)) {
                if (Player.EstaVivo) {
                    var tipomenu = Input.keyPressed(Key.Escape) ? TipoMenu.Principal : TipoMenu.Inventario;
                    CambiarMenu(tipomenu);
                    GameManager.SwitchMenu(this);
                }
                else {
                    Start();
                }
            }
            else {
                Mapa.Update(ElapsedTime, this);
                Player.Update(this);
                GameManager.Update(this);
            }

            PostUpdate();
        }

        public override void Dispose() {
            effect.Dispose();
            g_pRenderTarget.Dispose();
            g_pGlowMap.Dispose();
            g_pRenderTarget4Aux.Dispose();
            g_pRenderTarget4.Dispose();
            g_pVBV3D.Dispose();
            g_pDepthStencil.Dispose();
        }

        #region Renders
        private void RenderEscenaTarget() {
            effect.Technique = "DefaultTechnique";
            pOldRT = Device.GetRenderTarget(0);
            pSurf = g_pRenderTarget.GetSurfaceLevel(0);
            Device.SetRenderTarget(0, pSurf);
            pOldDS = Device.DepthStencilSurface;
            Device.DepthStencilSurface = g_pDepthStencil;
            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackgroundColor, 1.0f, 0);
            Device.BeginScene();
            Mapa.Render(this);
            Mapa.Instancia.ObjetosGlow.ForEach(o => o.Render(this));
            Device.EndScene();
            pSurf.Dispose();
        }

        private void RenderGlowMap() {
            effect.Technique = "DefaultTechnique";
            pSurf = g_pGlowMap.GetSurfaceLevel(0);
            Device.SetRenderTarget(0, pSurf);
            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            Device.BeginScene();
            Mapa.Instancia.ObjetosGlow.ForEach(o => o.RenderBrillo());
            Device.EndScene();
            pSurf.Dispose();
        }

        private void BlurGlowMap() {
            pSurf = g_pRenderTarget4.GetSurfaceLevel(0);
            Device.SetRenderTarget(0, pSurf);
            Device.BeginScene();
            effect.Technique = "DownFilter4";
            Device.VertexFormat = CustomVertex.PositionTextured.Format;
            Device.SetStreamSource(0, g_pVBV3D, 0);
            effect.SetValue("g_RenderTarget", g_pGlowMap);
            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            pSurf.Dispose();
            Device.EndScene();
            Device.DepthStencilSurface = pOldDS;
            for (var P = 0; P < cant_pasadas; ++P) {
                //Gaussian blur Horizontal
                pSurf = g_pRenderTarget4Aux.GetSurfaceLevel(0);
                Device.SetRenderTarget(0, pSurf);
                Device.BeginScene();
                effect.Technique = "GaussianBlurSeparable";
                Device.VertexFormat = CustomVertex.PositionTextured.Format;
                Device.SetStreamSource(0, g_pVBV3D, 0);
                effect.SetValue("g_RenderTarget", g_pRenderTarget4);
                Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                effect.Begin(FX.None);
                effect.BeginPass(0);
                Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                effect.EndPass();
                effect.End();
                pSurf.Dispose();
                Device.EndScene();
                pSurf = g_pRenderTarget4.GetSurfaceLevel(0);
                Device.SetRenderTarget(0, pSurf);
                pSurf.Dispose();

                //Gaussian blur Vertical
                Device.BeginScene();
                effect.Technique = "GaussianBlurSeparable";
                Device.VertexFormat = CustomVertex.PositionTextured.Format;
                Device.SetStreamSource(0, g_pVBV3D, 0);
                effect.SetValue("g_RenderTarget", g_pRenderTarget4Aux);
                Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                effect.Begin(FX.None);
                effect.BeginPass(1);
                Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                effect.EndPass();
                effect.End();
                Device.EndScene();
            }
        }

        private void RenderEscena() {
            Device.SetRenderTarget(0, pOldRT);
            Device.BeginScene();
            effect.Technique = "Final";
            Device.VertexFormat = CustomVertex.PositionTextured.Format;
            Device.SetStreamSource(0, g_pVBV3D, 0);
            effect.SetValue("g_RenderTarget", g_pRenderTarget);
            effect.SetValue("g_GlowMap", g_pRenderTarget4Aux);
            effect.SetValue("textura_casco", cascoTexture.D3dTexture);
            effect.SetValue("mostrarCasco", RenderCasco);
            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            Device.EndScene();
        }

        private void RenderPlayer() {
            Device.BeginScene();
            GameManager.Render(this);
            if (Player.EstaVivo)
                Player.Render();
            else
                PantallaMuerte.Render();
            Device.EndScene();
        }
        #endregion
        
        #region Logic

        private void Start() {
            var posInicial = GameManager.SpawnPlayer;
            Player = new Player(posInicial, 500, 1000, Input);
            EffectsPlayer = new EffectsPlayer(DirectSound);
            Camara = Player.CamaraInterna;
            CambiarMenu(TipoMenu.Guia);
            GameManager.SwitchMenu(this);
        }

        private void UpdateHud() {

        }

        private void RenderHUD() {
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            GameManager.CambiarMenu(tipoMenu);
        }

        public void UsarItem(ERecolectable item) {
            Player.Usar(item);
        }

        public void CheatItems() {
            Player.CheatItems();
            Play(SoundEffect.Coin);
        }

        public void CheatValor(string opcion) {
            Player.CheatValor(this, opcion);
            Play(SoundEffect.Coin);
        }

        public void FillAll() {
            Player.FillAll(this);
            Play(SoundEffect.Coin);
        }

        public void Salir() {
            Environment.Exit(0);
        }
        #endregion
        
    }
}