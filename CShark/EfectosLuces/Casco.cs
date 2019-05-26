using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace CShark.EfectosLuces
{
    public class Casco : IDisposable
    {
        private Effect Efecto;
        private TgcTexture TexturaCasco;
        private Texture RenderTarget2D;
        private Surface pOldRT;
        private Surface depthStencilOld;
        private VertexBuffer screenQuadVB;
        private Surface pSurf;
        private Device device;

        public Casco()
        {
            Init();
        }

        private void Init()
        {
            device = D3DDevice.Instance.Device;
            CustomVertex.PositionTextured[] screenQuadVertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            screenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);
            depthStencilOld = device.DepthStencilSurface;
            RenderTarget2D = new Texture(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            TexturaCasco = TgcTexture.createTexture(device, Game.Default.MediaDirectory + "Helmet2.png");
            Efecto = Efectos.Instancia.EfectoCasco;
        }

        public void RenderBeforeScene()
        {
            pOldRT = device.GetRenderTarget(0);
            pSurf = RenderTarget2D.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
        }

        public void RenderAfterScene()
        {
            pSurf.Dispose();

            device.SetRenderTarget(0, pOldRT);
            device.DepthStencilSurface = depthStencilOld;

            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, screenQuadVB, 0);

            Efecto.Technique = "CascoTechnique";
            Efecto.SetValue("render_target2D", RenderTarget2D);
            Efecto.SetValue("casco_textura", TexturaCasco.D3dTexture);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            Efecto.Begin(FX.None);
            Efecto.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            Efecto.EndPass();
            Efecto.End();
        }

        public void Dispose()
        {
            Efecto.Dispose();
            TexturaCasco.dispose();
            RenderTarget2D.Dispose();
            if (pOldRT != null) //asd
                pOldRT.Dispose();
        }
    }
}