using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Jugador;
using CShark.Model;
using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Items
{
    public abstract class Recolectable : IRecolectable {

        public TgcBoundingSphere EsferaCercania;
        public bool Recogido = false;

        public abstract TGCVector3 Posicion { get; }
        public abstract TGCVector3 Rotacion { get; }
        public abstract ERecolectable Tipo { get; }

        public abstract void Render();
        public abstract void Dispose();

        public Recolectable(TGCVector3 posicion) {
            EsferaCercania = new TgcBoundingSphere(posicion, 400f);
            EsferaCercania.setRenderColor(Color.Red);
        }

        public virtual void Update(GameModel game) {
            var player = game.Player;
            if (!Recogido) {
                //MoverEsferaCercania();
                if (PuedeRecoger(player)) {
                    EsferaCercania.setRenderColor(Color.Yellow);
                    if (player.Input.keyDown(Key.E)) {
                        Recogido = true;
                        player.Recoger(this);
                    }
                }
                else EsferaCercania.setRenderColor(Color.Red);
            }
        }

        private void MoverEsferaCercania() {
            var x = EsferaCercania.Position.X - Posicion.X;
            var y = EsferaCercania.Position.Y - Posicion.Y;
            var z = EsferaCercania.Position.Z - Posicion.Z;
            EsferaCercania.moveCenter(new TGCVector3(x, y, z));
        }

        public bool PuedeRecoger(Player player) {
            return TgcCollisionUtils.testPointSphere(EsferaCercania, player.Posicion);
        }       
    }
}
