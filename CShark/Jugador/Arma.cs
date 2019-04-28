using BulletSharp;
using CShark.Model;
using CShark.Terreno;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Jugador
{
    public class Arma
    {
        private TgcMesh Mesh;
        private TGCVector3 Offset;

        private TGCVector3 Posicion {
            get { return Mesh.Position; }
            set { Mesh.Position = value; }
        }

        private TGCVector3 Rotacion {
            get { return Mesh.Rotation; }
            set { Mesh.Rotation = value; }
        }

        private float LargoCañon;

        private TGCSphere Bala;
        private List<RigidBody> balas;

        public Arma() {
            Mesh = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Otros\Arma-TgcScene.xml").Meshes[0];
            Mesh.Scale = new TGCVector3(0.5f, 0.5f, 0.5f);
            Offset = new TGCVector3(50, 0, 0);
            Bala = new TGCSphere(50f, Color.Black, Posicion);
            Bala.updateValues();
            balas = new List<RigidBody>();
            LargoCañon = Mesh.BoundingBox.calculateSize().Z;
        }

        public void Disparar(TgcFpsCamera camara) {
            var ballBody = BulletRigidBodyFactory.Instance.CreateBall(100f, 1f, PuntaCañon());
            var dir = new TGCVector3(camara.LookAt.X - camara.Position.X, camara.LookAt.Y - camara.Position.Y, camara.LookAt.Z - camara.Position.Z).ToBulletVector3();
            dir.Normalize();
            ballBody.LinearVelocity = dir * 900;
            ballBody.LinearFactor = TGCVector3.One.ToBulletVector3();
            balas.Add(ballBody);
            Mapa.Instancia.AgregarMuerto(ballBody);
        }

        private float Grados180 = (float)Math.PI;

        public void Update(GameModel game) {
            var player = game.Player;
            var cam = player.CamaraInterna;
            Posicion = player.Posicion + Offset;
            Rotacion = new TGCVector3(cam.updownRot, cam.leftrightRot, 0);

            if (game.Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                Disparar(player.CamaraInterna);
        }

        public void Render() {
            Mesh.Render();
            foreach (var ball in balas)
            {
                Bala.Transform = TGCMatrix.Scaling(10, 10, 10) * new TGCMatrix(ball.InterpolationWorldTransform);
                Bala.Render();
            }
        }

        private TGCVector3 PuntaCañon() {
            return new TGCVector3(Posicion.X, Posicion.Y, Posicion.Z + LargoCañon);
        }

    }
}
