﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Jugador;
using CShark.Model;
using CShark.Terreno;
using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using static TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox;

namespace CShark.Items
{
    public abstract class Recolectable : IRecolectable
    {
        public abstract TGCVector3 Posicion { get; }
        public abstract TGCVector3 Rotacion { get; }
        public abstract ERecolectable Tipo { get; }
        public abstract TgcBoundingAxisAlignBox Box { get; }
        public TgcBoundingSphere EsferaCercania { get; }

        public bool Recogido = false;

        public abstract void Render(GameModel game);
        public abstract void Dispose();

        public Recolectable(TGCVector3 posicion) {
            EsferaCercania = new TgcBoundingSphere(posicion, 700f);
        }

        public virtual void Update(GameModel game) {
            var player = game.Player;
            if (!Recogido && PuedeRecoger(player) && player.Input.keyDown(Key.E)) {
                Recogido = true;
                player.Recoger(this);
            }
        }

        public bool PuedeRecoger(Player player) {
            return player.PuedeRecoger(this);
        }

    }
}
