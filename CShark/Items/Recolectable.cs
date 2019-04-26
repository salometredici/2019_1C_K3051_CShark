using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Jugador;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items
{
    public abstract class Recolectable : IRecolectable
    {
        private TgcMesh Mesh;

        public Recolectable(string mesh) {
            var ruta = Game.Default.MediaDirectory + "Recolectables\\" + mesh + "-TgcScene.xml";
            Mesh = new TgcSceneLoader().loadSceneFromFile(ruta).Meshes[0];
            Init();
        }

        public void Desaparecer() {
            Mesh.Dispose();
        }

        public void Render() {
            Mesh.Render();
            collisionableSphere.Render();
            collisionableMeshAABB.BoundingBox.Render();
            collisionableCylinder.Render();
        }

        public bool EstaCerca(Player player) {
            throw new NotImplementedException();
        }

        private const float PICKING_TIME = 0.5f;
        private readonly Color collisionColor = Color.Red;

        private readonly Color noCollisionColor = Color.Yellow;
        private TgcBoundingCylinder colliderCylinder;
        private TgcBoundingCylinderFixedY colliderCylinderFixedY;
        private TgcBoundingSphere collisionableSphere;
        private TgcMesh collisionableMeshAABB;
        private TgcBoundingCylinderFixedY collisionableCylinder;

        public void Init() {
            colliderCylinder = new TgcBoundingCylinder(TGCVector3.Empty, 2, 4);
            colliderCylinder.setRenderColor(Color.LimeGreen);
            colliderCylinderFixedY = new TgcBoundingCylinderFixedY(TGCVector3.Empty, 2, 4);
            colliderCylinderFixedY.setRenderColor(Color.LimeGreen);

            collisionableMeshAABB = Mesh;
            collisionableMeshAABB.Scale = new TGCVector3(0.1f, 0.1f, 0.1f);
            collisionableMeshAABB.Position = new TGCVector3(6, 0, -2);
            collisionableCylinder = new TgcBoundingCylinderFixedY(new TGCVector3(-6, 0, 0), 2, 2);
            collisionableSphere = new TgcBoundingSphere(new TGCVector3(-3, 0, 10), 3);
            colliderCylinder.Center = Mesh.Position;
            colliderCylinderFixedY.Center = Mesh.Position;
        }


    }
}
