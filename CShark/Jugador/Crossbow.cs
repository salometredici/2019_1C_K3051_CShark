using BulletSharp;
using BulletSharp.Math;
using CShark.Managers;
using CShark.Model;
using CShark.NPCs.Enemigos;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Linq;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Jugador
{
    public class Crossbow : Arma
    {
        private List<RigidBody> Municiones;
        private TgcMesh MeshHarpoon;

        public Crossbow() : base() {
            Municiones = new List<RigidBody>();
            Init();
        }

        private void Init() {
            var loader = new TgcSceneLoader();
            var path = Game.Default.MediaDirectory + @"Otros\";
            var meshCrossbow = loader.loadSceneFromFile(path + "Crossbow-TgcScene.xml").Meshes[0];
            var meshHarpoon = loader.loadSceneFromFile(path + "Harpoon-TgcScene.xml").Meshes[0];
            Mesh = meshCrossbow;
            MeshHarpoon = meshHarpoon;
            Mesh.AutoTransformEnable = false;
            MeshHarpoon.AutoTransformEnable = false;
        }

        public override void Update(GameModel game) {
            base.Update(game);
            Tiburon tibu = null;
            foreach (var bala in Municiones.ToList() /*bien villero para poder pisar la lista en el loop*/) {
                if ((tibu = Mapa.Instancia.BalaColisiona(bala)) != null){
                    tibu.RecibirDaño(50f);
                    Municiones.Remove(bala);
                }
            }
        }

        public override void Render() {
            base.Render();
            foreach (var bala in Municiones) {
                MeshHarpoon.Transform = new TGCMatrix(bala.InterpolationWorldTransform);
                MeshHarpoon.Render();
            }
        }

        public override void Atacar(Player player) {
            var camara = player.CamaraInterna;
            var puntaCañon = CalcularPuntaCañon(player);
            var harpoonBody = CrearHarpoonBody(player, puntaCañon);
            var direccionDisparo = new TGCVector3(camara.LookAt.X - camara.Position.X, camara.LookAt.Y - camara.Position.Y, camara.LookAt.Z - camara.Position.Z).ToBulletVector3();
            direccionDisparo.Normalize();
            harpoonBody.LinearVelocity = direccionDisparo * 6000;
            //harpoonBody.LinearFactor = TGCVector3.One.ToBulletVector3();
            Municiones.Add(harpoonBody);
            Mapa.Instancia.AgregarBody(harpoonBody);
            LimitarCantidad();
        }

        private void LimitarCantidad() {
            if (Municiones.Count() > 5) {
                var primero = Municiones.First();
                Municiones.Remove(primero);
                Mapa.Instancia.SacarBody(primero);
            }
        }

        private RigidBody CrearHarpoonBody(Player player, TGCVector3 punta) {
            var size = MeshHarpoon.BoundingBox.calculateSize();
            var rotacionY = player.CamaraInterna.leftrightRot + (float)Math.PI;
            var rotacionZ = -player.CamaraInterna.updownRot; //un fix visual pequeño
            return BulletRigidBodyFactory.Instance.CreateBox(size, 0.2f, punta, rotacionY, rotacionZ, 0, 0, false);
        }

        private TGCVector3 CalcularPuntaCañon(Player player) {
            var rotY = player.CamaraInterna.leftrightRot + FastMath.PI;
            var rotX = -player.CamaraInterna.updownRot;
            var rotation = TGCMatrix.RotationYawPitchRoll(rotY, rotX, 0);
            var offset = TGCMatrix.Translation(Offset + new TGCVector3(0, 0, 100f));
            var position = TGCMatrix.Translation(player.Posicion);
            return TGCVector3.transform(TGCVector3.Empty, offset * rotation * position);
        }

        private Vector3 DireccionDisparo(Player player) {
            var punta = CalcularPuntaCañon(player);
            var lookAt = player.CamaraInterna.LookAt;
            return (punta - lookAt).ToBulletVector3();
        }
    }
}
