﻿using CShark.Jugador;
using CShark.Model;
using CShark.Objetos;
using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using Effect = Microsoft.DirectX.Direct3D.Effect;

namespace CShark.Items
{
    public abstract class Recolectable : IRecolectable
    {
        public abstract TGCVector3 Posicion { get; }
        public abstract TGCVector3 Rotacion { get; }
        public abstract ERecolectable Tipo { get; }
        public TgcBoundingSphere EsferaCercania { get; }
        public abstract TgcBoundingAxisAlignBox BoundingBox { get; }
        public abstract Material Material { get; }
        public abstract TgcMesh Mesh { get; }
        public bool Enabled { get; set; }
        public bool Recogido = false;

        public abstract void Render(GameModel game);
        public abstract void Dispose();
        public abstract void Desaparecer();

        public Recolectable(TGCVector3 posicion) {
            EsferaCercania = new TgcBoundingSphere(posicion, 700f);
        }

        public virtual void Update(GameModel game) {
            var player = game.Player;
            if (!Recogido && PuedeRecoger(player) && player.Input.keyDown(Key.E)) {
                Recogido = true;
                player.Recoger(this);
                Desaparecer();
            }
        }

        public bool PuedeRecoger(Player player) {
            return player.PuedeRecoger(this);
        }

        public void RenderOscuro() {

        }
    }
}
