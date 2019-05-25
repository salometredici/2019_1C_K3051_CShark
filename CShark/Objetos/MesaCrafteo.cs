﻿using CShark.EfectosLuces;
using CShark.Jugador;
using CShark.Model;
using CShark.Terreno;
using Microsoft.DirectX.DirectInput;
using System;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using Effect = Microsoft.DirectX.Direct3D.Effect;

namespace CShark.Objetos
{
    public class MesaCrafteo : IRenderable
    {
        private TgcBoundingSphere EsferaCercania;
        private TgcText2D TextoPresione;
        private bool MostrarTexto = false;
        public TGCVector3 Size => Mesh.BoundingBox.calculateSize();
        public TGCVector3 Position => Mesh.BoundingBox.calculateBoxCenter();

        public Material Material { get; }
        public TgcMesh Mesh { get; }


        public MesaCrafteo() {
            Mesh = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Mesa-TgcScene.xml").Meshes[0];
            EsferaCercania = new TgcBoundingSphere(Position, 500f);
            EsferaCercania.setRenderColor(Color.Red);
            InicializarTexto();
            Material = Materiales.Madera;
            var body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(Mesh);
            Mapa.Instancia.AgregarBody(body);
        }

        private void InicializarTexto() {
            TextoPresione = new TgcText2D();
            TextoPresione.Text = "Presione E para craftear";
            var w = D3DDevice.Instance.Device.Viewport.Width;
            var h = D3DDevice.Instance.Device.Viewport.Height;
            TextoPresione.Position = new Point(0, h - 100);
            TextoPresione.Align = TgcText2D.TextAlign.CENTER;
            TextoPresione.Color = Color.Yellow;
            TextoPresione.Size = new Size(w, 50);
            TextoPresione.changeFont(new Font("Arial", 30f, FontStyle.Bold));
        }

        public void Update(GameModel game) {
            Efectos.ActualizarLuces(Mesh.Effect, Material, game.Camara.Position);
            if (EstaCerca(game.Player)) {
                EsferaCercania.setRenderColor(Color.Yellow);
                MostrarTexto = true;
                if (game.Input.keyPressed(Key.E) && game.Player.EstaVivo) {
                    game.CambiarMenu(UI.TipoMenu.Crafteo);
                    game.GameManager.SwitchMenu(game); //mira ese acoplamiento
                    MostrarTexto = false;
                }
            }
            else {
                MostrarTexto = false;
                EsferaCercania.setRenderColor(Color.Red);
            }
        }

        public void Dispose() {
            Mesh.Render();
            EsferaCercania.Dispose();
        }

        public bool EstaCerca(Player player) {
            return TgcCollisionUtils.testPointSphere(EsferaCercania, player.Posicion);
        }

        public void Render() {
            Mesh.Render();
            EsferaCercania.Render();
            if (MostrarTexto)
                TextoPresione.render();
        }
    }
}