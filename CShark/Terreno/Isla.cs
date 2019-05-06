using BulletSharp;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Terreno
{
    public class Isla
    {
        private List<TgcMesh> objetosIsla;
        private Octree octree;
        private TgcMesh terreno;
        private TgcMesh barco;
        private TgcFrustum frustum;

        public Isla() {
            Init();
        }

        public void Init()
        {
            frustum = new TgcFrustum();

            //Cargar escenario de Isla y barco
            var loader = new TgcSceneLoader();
            var scene = loader.loadSceneFromFile(Game.Default.MediaDirectory + "Isla-TgcScene.xml");
            var boat = loader.loadSceneFromFile(Game.Default.MediaDirectory + "bot-TgcScene.xml");
            
            //Separar el Terreno del resto de los objetos
            var list1 = new List<TgcMesh>();
            scene.separeteMeshList(new[] { "Terreno" }, out list1, out objetosIsla);
            terreno = list1[0];

            //Reposicionar toda la escena al nivel del mar
            foreach(var item in scene.Meshes)
            {
                item.Position += new TGCVector3(0,2900f,0);
            }

            barco = boat.Meshes[0];
            barco.Position = new TGCVector3(1500f,2950f,0);
            barco.Scale = new TGCVector3(0.05f, 0.05f, 0.05f);

            //Crear Octree
            octree = new Octree();
            octree.create(objetosIsla, scene.BoundingBox);
            octree.createDebugOctreeMeshes();

        }

        public void Render()
        {
            barco.Render();
            terreno.Render();
            octree.render(frustum, false);
        }

        public void Dispose()
        {
            terreno.Dispose();
            foreach (var mesh in objetosIsla)
            {
                mesh.Dispose();
            }
            barco.Dispose();
        }

    }
}
