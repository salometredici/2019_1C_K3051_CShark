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
        private TgcSkyBox Skybox;
        private TGCVector3 center;

        public SkyBox(TGCVector3 centro)
        {
            Inicializar(centro);
        }

        public void Inicializar(TGCVector3 centro)
        {
            Skybox = new TgcSkyBox
            {
                Center = centro + new TGCVector3(0,200,0),
                Size = new TGCVector3(10000,5000,10000)
            };
            var texturesPath = Game.Default.MediaDirectory + "Textures\\UnderwaterSkybox\\";
            Skybox.setFaceTexture(SkyFaces.Up, texturesPath + "blue-texture.png");
            Skybox.setFaceTexture(SkyFaces.Down, texturesPath + "seafloor.jpg");
            Skybox.setFaceTexture(SkyFaces.Left, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Right, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Front, texturesPath + "side.jpg");
            Skybox.setFaceTexture(SkyFaces.Back, texturesPath + "side.jpg");
            Skybox.SkyEpsilon = 50f;
            Skybox.Init();
        }

        public void Render(TGCVector3 playerPosition)
        {
            center = Skybox.Center + playerPosition;
            var traslationMatrix = TGCMatrix.Translation(center);
            foreach (var face in Skybox.Faces)
            {
                face.Transform = TGCMatrix.Identity * traslationMatrix;
                face.Render();
            }
        }

    }
}
