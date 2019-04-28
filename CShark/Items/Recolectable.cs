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
        protected TgcMesh Mesh;

        public TgcBoundingSphere EsferaCercania;

        protected TGCVector3 Posicion {
            get {
                return Mesh.Position;
            }
            set {
                Mesh.Position = value;
            }
        }

        protected TGCVector3 Rotacion {
            get {
                return Mesh.Rotation;
            }
            set {
                Mesh.Rotation = value;
            }
        }

        public Recolectable(string mesh, TGCVector3 posicion) {
            var ruta = Game.Default.MediaDirectory + "Recolectables\\" + mesh + "-TgcScene.xml";
            Mesh = new TgcSceneLoader().loadSceneFromFile(ruta).Meshes[0];
            Mesh.Position = posicion;
            EsferaCercania = new TgcBoundingSphere(Posicion, 100f);
            EsferaCercania.setRenderColor(Color.Red);
        }

        public void Desaparecer() {
            Mesh.Dispose();
        }

        public virtual void Update(Player player) {
            MoverEsferaCercania();
            if (EstaCerca(player))
                EsferaCercania.setRenderColor(Color.Yellow);
            else
                EsferaCercania.setRenderColor(Color.Red);
        }

        public virtual void Render() {
            Mesh.Render();
            EsferaCercania.Render();
        }

        public bool EstaCerca(Player player) {
            return TgcCollisionUtils.testPointSphere(EsferaCercania, player.Posicion);
        }

        private void MoverEsferaCercania() {
            var x = EsferaCercania.Position.X - Posicion.X;
            var y = EsferaCercania.Position.Y - Posicion.Y;
            var z = EsferaCercania.Position.Z - Posicion.Z;
            EsferaCercania.moveCenter(new TGCVector3(x, y, z));
        }
    }
}
