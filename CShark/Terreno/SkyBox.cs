using CShark.EfectosLuces;
using CShark.Jugador;
using CShark.Model;
using CShark.Objetos;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using static TGC.Core.Terrain.TgcSkyBox; //para enums
using Material = CShark.Objetos.Material;

namespace CShark.Terreno
{
    public class SkyBox : Iluminable
    {
        private TgcSkyBox SkyboxUnderwater;
        private TgcSkyBox SkyboxIsland;
        public List<TgcMesh> FacesToRender;
        private string texturesPath;

        public SkyBox(TGCVector3 centro) : base(Materiales.Normal)
        {
            Inicializar(centro);
        }

        public void Inicializar(TGCVector3 centro)
        {
            FacesToRender = new List<TgcMesh>();
            texturesPath = Game.Default.MediaDirectory + @"Textures\";
            SetUnderWaterSkybox(centro);
            SetIslandSkybox(centro + new TGCVector3(0, 32500, 0));
            SkyboxUnderwater.Init();
            SkyboxIsland.Init();
            CargarCaras();
        }

        private void SetUnderWaterSkybox(TGCVector3 centro)
        {
            SkyboxUnderwater = new TgcSkyBox
            {
                Center = centro,
                Size = new TGCVector3(350000, 30000, 350000)
            };
            SkyboxUnderwater.setFaceTexture(SkyFaces.Up, texturesPath + @"UnderwaterSkybox\up1.png");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Down, texturesPath + @"UnderwaterSkybox\seafloor.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Left, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Right, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Front, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.setFaceTexture(SkyFaces.Back, texturesPath + @"UnderwaterSkybox\side.jpg");
            SkyboxUnderwater.SkyEpsilon = 100f;
        }

        private void SetIslandSkybox(TGCVector3 centroIsla)
        {
            SkyboxIsland = new TgcSkyBox
            {
                Center = centroIsla,
                Size = new TGCVector3(350000, 36000, 350000)
            };
            SkyboxIsland.setFaceTexture(SkyFaces.Left, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_lf.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Back, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_ft.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Right, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_rt.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Front, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_bk.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Up, texturesPath + @"SkyBox-LostAtSeaDay\lostatseaday_up.jpg");
            SkyboxIsland.setFaceTexture(SkyFaces.Down, texturesPath + @"SkyBox-LostAtSeaDay\skybox-island-water.png");
            SkyboxIsland.SkyEpsilon = 100f;
        }
        
        private void CargarCaras() {
            var faces = SkyboxUnderwater.Faces.Concat(SkyboxIsland.Faces).ToList();
            var marIsla = SkyboxIsland.Faces.FirstOrDefault(f => f.Name.Equals("SkyBox-Down"));
            var marTerreno = SkyboxUnderwater.Faces.FirstOrDefault(f => f.Name.Equals("SkyBox-Up"));
            faces.Remove(marIsla); //parte de abajo de skybox superficial
            faces.Remove(marTerreno); //parte de arriba de skybox agua
            FacesToRender = faces.ToList();
            FacesToRender.ForEach(face => {
                face.Effect = this.Efecto;
                face.Technique = this.Tecnica;
            });
        }

        public override void Update(GameModel game) {
            //...
        }

        public override void Render(GameModel game)
        {
            base.Render(game);
            FacesToRender.ForEach(f => f.Render());
        }

        public override void Dispose() {
            SkyboxIsland.Dispose();
            SkyboxUnderwater.Dispose();
        }

        public void CambiarEfecto(Effect efecto, string technique) {
            this.Efecto = efecto;
            this.Tecnica = technique;
            FacesToRender.ForEach(f => {
                f.Effect = efecto;
                f.Technique = technique;
            });
        }

        public TGCVector3 Center {
            get {
                return (SkyboxIsland.Center + SkyboxUnderwater.Center) * 0.5f;
            }
        }

        public TGCVector3 Size {
            get {
                return new TGCVector3(SkyboxUnderwater.Size.X, SkyboxUnderwater.Size.Y + SkyboxIsland.Size.Y, SkyboxUnderwater.Size.Z);
            }
        }
    }
}
