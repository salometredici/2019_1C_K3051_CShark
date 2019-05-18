using BulletSharp;
using CShark.Model;
using CShark.Terreno;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Jugador
{
    public abstract class Arma
    {
        protected TgcMesh Mesh;
        protected float RotacionY;
        protected TGCVector3 Offset;
        protected float Oscilacion;

        public Arma() {
            Offset = new TGCVector3(10f, -10f, 30f);
            RotacionY = 0f;
            Oscilacion = 0;
        }

        public abstract void Atacar(Player player);

        public virtual void Update(GameModel game) {
            Transform = ArmaTransform(game.Player);
            if (game.Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                Atacar(game.Player);
        }

        public virtual void Render() {
            Mesh.Render();
        }

        public TGCMatrix ArmaTransform(Player player) {
            var scale = TGCMatrix.Scaling(0.5f, 0.5f, 0.5f);
            var rotY = player.CamaraInterna.leftrightRot + FastMath.PI;
            var rotX = -player.CamaraInterna.updownRot;
            var rotation = TGCMatrix.RotationYawPitchRoll(rotY, rotX, 0);
            var offset = TGCMatrix.Translation(Offset);
            var position = TGCMatrix.Translation(player.Posicion);
            return scale * offset * rotation * position;
        }

        public TGCVector3 Posicion {
            get { return Mesh.Position; }
            set { Mesh.Position = value; }
        }

        public TGCVector3 Rotacion {
            get { return Mesh.Rotation; }
            set { Mesh.Rotation = value; }
        }

        public TGCVector3 Escala {
            get { return Mesh.Scale; }
            set { Mesh.Scale = value; }
        }

        protected TGCMatrix Transform {
            get { return Mesh.Transform; }
            set { Mesh.Transform = value; }
        }

    }
}
