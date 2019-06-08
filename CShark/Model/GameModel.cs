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
        private PantallaMuerte PantallaMuerte;
        private Mapa Mapa => Mapa.Instancia;
        private bool RenderCasco = true;
        private Device Device => D3DDevice.Instance.Device;
        #endregion

        #region Constructors
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        #endregion

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

            this.SetRenderTarget();

            Device.BeginScene();

            GameManager.Render(this);
            Mapa.Render(this);

            if (Player.EstaVivo)
                Player.Render();
            else
                PantallaMuerte.Render();

            this.RenderFPS();

            Device.EndScene();

            this.ShowRenderTarget();
            this.DrawPostProcess();

            Device.BeginScene();
            HUD.Instancia.Render();
            Device.EndScene();
            Device.Present();
        }

        public override void Dispose()
        {
            GameManager.Dispose();
            Mapa.Instancia.Dispose();
            GameManager.Dispose();
        }

        #region Logic
        public override void Init() {
            Cursor.Hide();
            PantallaMuerte = new PantallaMuerte();
            GameManager = new GameManager();
            this.InitPostProcess();
            Start();
        }

        private void Start() {
            var posInicial = GameManager.SpawnPlayer;
            Player = new Player(posInicial, 500, 1000, Input);
            Camara = Player.CamaraInterna;
            CambiarMenu(TipoMenu.Guia);
            GameManager.SwitchMenu(this);
        }

        private void UpdateHud() {

        }

        private void RenderHUD() {
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
        #endregion

        #region Shaders de Post Procesado

        private TgcTexture flareTexture;
        private TgcTexture cascoTexture;
        private Surface depthStencil; // Depth-stencil buffer
        private Surface depthStencilOld;
        private Effect effect;
        private Surface pOldRT;
        private Surface pSurf;
        private Texture renderTarget2D;
        private VertexBuffer screenQuadVB;

        public void InitPostProcess() {
            CustomVertex.PositionTextured[] screenQuadVertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            screenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, D3DDevice.Instance.Device, Usage.Dynamic | Usage.WriteOnly,
                CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);
            renderTarget2D = new Texture(D3DDevice.Instance.Device, D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth,
                D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            depthStencil = D3DDevice.Instance.Device.CreateDepthStencilSurface(D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth,
                    D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight, DepthFormat.D24S8, MultiSampleType.None, 0, true);
            depthStencilOld = D3DDevice.Instance.Device.DepthStencilSurface;
            effect = TGCShaders.Instance.LoadEffect(ShadersDir + "PostProcesado.fx");
            effect.Technique = "General";
            flareTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + @"Textures\rayoSol.png");
            cascoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + @"Textures\casco.png");
        }

        private void SetRenderTarget() {
            ClearTextures();
            pOldRT = D3DDevice.Instance.Device.GetRenderTarget(0);
            pSurf = renderTarget2D.GetSurfaceLevel(0);
            Device.SetRenderTarget(0, pSurf);
            Device.DepthStencilSurface = depthStencil;
            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackgroundColor, 1.0f, 0);
        }

        private void ShowRenderTarget() {
            pSurf.Dispose();
            Device.SetRenderTarget(0, pOldRT);
            Device.DepthStencilSurface = depthStencilOld;
        }

        private void DrawPostProcess() {
            var sol = Mapa.Instancia.Sol;
            var matWorldView = D3DDevice.Instance.Device.Transform.View;
            var matWorldViewProj = matWorldView * D3DDevice.Instance.Device.Transform.Projection;
            Device.BeginScene();
            Device.VertexFormat = CustomVertex.PositionTextured.Format;
            Device.SetStreamSource(0, screenQuadVB, 0);
            effect.Technique = "General";
            effect.SetValue("render_target2D", renderTarget2D);
            effect.SetValue("textura_flare", flareTexture.D3dTexture);
            effect.SetValue("textura_casco", cascoTexture.D3dTexture);
            effect.SetValue("mostrarCasco", RenderCasco);
            effect.SetValue("posicionSol", TGCVector3.Vector3ToVector4(sol.Posicion));
            effect.SetValue("posicionPlayer", TGCVector3.Vector3ToVector4(Player.Posicion));
            effect.SetValue("matWorld", TGCMatrix.Identity.ToMatrix());
            effect.SetValue("matWorldView", matWorldView);
            effect.SetValue("matWorldViewProj", matWorldViewProj);

            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackgroundColor, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();
            Device.EndScene();
        }

        #endregion

    }
}