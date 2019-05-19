﻿using BulletSharp;
using CShark.EfectosLuces;
using CShark.Fisica;
using CShark.Jugador;
using CShark.Model;
using CShark.Terreno;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Barco : IRenderable
    {
        private RigidBody Body;
        public Material Material { get; }
        public TgcMesh Mesh { get; }

        public Barco() {
            Mesh = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Barco-TgcScene.xml").Meshes[0];
            Body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(Mesh);
            Mapa.Instancia.AgregarBody(Body);
            Material = Materiales.Metal;
        }
        
        public void Update(GameModel game) {
            Efectos.ActualizarLuces(Mesh.Effect, Material, game.Camara.Position);
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Render() {
            Mesh.Render();

        }
    }
}
