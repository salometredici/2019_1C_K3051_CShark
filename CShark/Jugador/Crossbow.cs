﻿using BulletSharp;
using CShark.Managers;
using CShark.Model;
using CShark.Terreno;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

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
            var meshHarpoon =  loader.loadSceneFromFile(path + "Harpoon-TgcScene.xml").Meshes[0];
            Mesh = meshCrossbow;
            MeshHarpoon = meshHarpoon;
            Mesh.AutoTransformEnable = false;
            MeshHarpoon.AutoTransformEnable = false;
        }

        public override void Update(GameModel game) {
            base.Update(game);
            foreach (var bala in Municiones)
            {
                if (Mapa.Instancia.Colisionan(bala, FaunaManager.Tiburon.Body)) { 
                    FaunaManager.Tiburon.RecibirDaño(50f);
                }
            }
        }

        public override void Render() {
            base.Render();
            foreach (var bala in Municiones)
            {
                MeshHarpoon.Transform = new TGCMatrix(bala.InterpolationWorldTransform);
                MeshHarpoon.Render();
            }
        }

        public override void Atacar(Player player) {
            var camara = player.CamaraInterna;
            var puntaCañon = CalcularPuntaCañon();
            var harpoonBody = CrearHarpoonBody();
            var direccionDisparo = new TGCVector3(camara.LookAt.X - camara.Position.X, camara.LookAt.Y - camara.Position.Y, camara.LookAt.Z - camara.Position.Z).ToBulletVector3();
            direccionDisparo.Normalize();
            harpoonBody.LinearVelocity = direccionDisparo * 900;
            harpoonBody.LinearFactor = TGCVector3.One.ToBulletVector3();
            Municiones.Add(harpoonBody);
            Mapa.Instancia.AgregarBody(harpoonBody);
        }

        private RigidBody CrearHarpoonBody() {
            var pmin = MeshHarpoon.BoundingBox.PMin;
            var pmax = MeshHarpoon.BoundingBox.PMax;
            var size = new TGCVector3(pmax.X - pmin.X, pmax.Y - pmin.Y, pmax.Z - pmin.Z);
            var posicion = CalcularPuntaCañon();
            return BulletRigidBodyFactory.Instance.CreateBox(size, 10f, posicion, 0, 0, 0, 1, false);
        }

        private TGCVector3 CalcularPuntaCañon() {
            return TGCVector3.transform(Posicion, Transform);
        }
    }
}
