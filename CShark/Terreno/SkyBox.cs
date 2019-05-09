using CShark.Jugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using static TGC.Core.Terrain.TgcSkyBox; //para enums

namespace CShark.Terreno
{
    public class SkyBox : IDisposable
    {
        private TgcSkyBox SkyboxUnderwater;
        private TgcSkyBox SkyboxIsland;
        private List<TgcMesh> FacesToRender;
        private TgcSkyBox SkyboxToRender;
        private string texturesPath;

        public TGCVector3 Center => SkyboxToRender.Center;
        public TGCVector3 Size => SkyboxToRender.Size;        

        public SkyBox(TGCVector3 centro)
        {
            Inicializar(centro);
        }

        public void Inicializar(TGCVector3 centro)
        {
            FacesToRender = new List<TgcMesh>();
            texturesPath = Game.Default.MediaDirectory + @"Textures\";
            SetUnderWaterSkybox(centro);
            SetIslandSkybox(centro + new TGCVector3(0, 4000f, 0));
            SkyboxUnderwater.Init();
            SkyboxIsland.Init();
            SetSkybox(new TGCVector3(1500f, 3050f, 0)); //esto cambiar despues..
        }

        private void SetUnderWaterSkybox(TGCVector3 centro)
        {
            SkyboxUnderwater = new TgcSkyBox
            {
                Center = centro,
                Size = new TGCVector3(10000, 5000, 10000)
            };
            SkyboxUnderwater.setFaceTexture(SkyFaces.Up, texturesPath + @"UnderwaterSkybox\up1.png");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Down, texturesPath + @"UnderwaterSkybox\seafloor.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Left, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Right, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Front, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Back, texturesPath + @"UnderwaterSkybox\side.jpg");
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
            SkyboxIsland.setFaceTexture(SkyFaces.Left, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_lf.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Back, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_ft.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Right, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_rt.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Front, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_bk.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Up, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_up.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Down, texturesPath + @"SkyBox-LostAtSeaDay\skybox-island-water.png");
            SkyboxIsland.SkyEpsilon = 50f;
        }

        private bool CambiarSkybox(TGCVector3 playerPosition) {
            return SkyboxToRender.Equals(SkyboxUnderwater) && playerPosition.Y >= 2875
                || SkyboxToRender.Equals(SkyboxIsland) && playerPosition.Y < 2875;
        }

        private void SetSkybox(TGCVector3 playerPosition) {
            bool aboveWater = playerPosition.Y >= 2875;
            SkyboxToRender =  aboveWater ? SkyboxIsland : SkyboxUnderwater;
            var faces = SkyboxToRender.Faces.ToList();
            var caraNoRenderear = aboveWater
                ? faces.First(f => f.Name.Equals("SkyBox-Down"))
                : faces.First(f => f.Name.Equals("SkyBox-Up"));
            faces.Remove(caraNoRenderear);
            FacesToRender = faces;
        }

        public void Update(Player player) {
            if (CambiarSkybox(player.Posicion)) //para no recrear la lista siempre, + performance
                SetSkybox(player.Posicion);
        }

        public void Render()
        {
            FacesToRender.ForEach(f => f.Render());
        }

        public void Dispose() {
            SkyboxIsland.Dispose();
            SkyboxUnderwater.Dispose();
        }
    }
}
