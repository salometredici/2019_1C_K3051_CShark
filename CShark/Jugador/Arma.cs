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

        public Arma(string mesh) {
            Mesh = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Otros\"+mesh+"-TgcScene.xml").Meshes[0];
            Mesh.Scale = new TGCVector3(0.5f, 0.5f, 0.5f);      
            Offset = new TGCVector3(50f, 0, 100f);
            RotacionY = 0.1f;
            Oscilacion = 0;
            Mesh.AutoTransformEnable = false;
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
            var viewMatrix = player.CamaraInterna.GetViewMatrix();
            var fpsMatrixInv = TGCMatrix.Invert(viewMatrix);
            float z = (float)Math.Cos(Oscilacion) / 2;
            float y = (float)Math.Sin(2 * Oscilacion) / 16;
            var offset = TGCMatrix.Translation(Offset + new TGCVector3(0, y, z));
            var scale = TGCMatrix.Scaling(0.5f, 0.5f, 0.5f);
            var rotationY = TGCMatrix.RotationY(RotacionY);  
            return scale * rotationY * offset * fpsMatrixInv;
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
