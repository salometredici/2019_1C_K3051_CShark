using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.Terrain;

namespace CShark.Terreno
{
    public class SkyBox : TgcSkyBox
    {
        private TgcSkyBox SkyboxUnderwater;
        private TgcSkyBox SkyboxIsland;

        private string texturesPath = Game.Default.MediaDirectory + "Textures\\";

        public SkyBox(TGCVector3 centro)
        {
            Inicializar(centro);
        }

        public void Inicializar(TGCVector3 centro)
        {
            SetUnderWaterSkybox(centro);
            SetIslandSkybox(centro + new TGCVector3(0,4000f,0));
            SkyboxUnderwater.Init();
            SkyboxIsland.Init();
        }

        private void SetUnderWaterSkybox(TGCVector3 centro)
        {
            SkyboxUnderwater = new TgcSkyBox
            {
                Center = centro,
                Size = new TGCVector3(10000, 5000, 10000)
            };
            SkyboxUnderwater.setFaceTexture(SkyFaces.Up, texturesPath + "UnderwaterSkybox\\up1.png");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Down, texturesPath + "UnderwaterSkybox\\seafloor.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Left, texturesPath + "UnderwaterSkybox\\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Right, texturesPath + "UnderwaterSkybox\\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Front, texturesPath + "UnderwaterSkybox\\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Back, texturesPath + "UnderwaterSkybox\\side.jpg");
            SkyboxUnderwater.SkyEpsilon = 50f;
            SkyboxUnderwater.Center += new TGCVector3(0,250f,0);
        }

        private void SetIslandSkybox(TGCVector3 centroIsla)
        {
            SkyboxIsland = new TgcSkyBox
            {
                Center = centroIsla,
                Size = new TGCVector3(10000, 2500, 10000)
            };
            SkyboxIsland.setFaceTexture(SkyFaces.Left, texturesPath + "SkyBox-LostAtSeaDay\\lostatseaday_lf.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Back, texturesPath + "SkyBox-LostAtSeaDay\\lostatseaday_ft.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Right, texturesPath + "SkyBox-LostAtSeaDay\\lostatseaday_rt.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Front, texturesPath + "SkyBox-LostAtSeaDay\\lostatseaday_bk.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Up, texturesPath + "SkyBox-LostAtSeaDay\\lostatseaday_up.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Down, texturesPath + "SkyBox-LostAtSeaDay\\skybox-island-water.png");
            SkyboxIsland.SkyEpsilon = 50f;
        }

        public void Render(TGCVector3 playerPosition)
        {
            /*center = Skybox.Center + playerPosition;
            var traslationMatrix = TGCMatrix.Translation(center);*/
            var SkyboxToRender = playerPosition.Y < 2875 ? SkyboxUnderwater : SkyboxIsland;
            foreach (var face in SkyboxToRender.Faces)
            {
                // face.Transform = TGCMatrix.Identity * traslationMatrix;
                face.Render();
            }
        }
    }
}
