﻿using System;
using System.Collections.Generic;
using System.Drawing;
using BulletSharp;
using CShark.EfectosLuces;
using CShark.Fisica.Colisiones;
using CShark.Jugador;
using CShark.Model;
using CShark.Objetos;
using CShark.Utilidades;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using TGC.Core.BulletPhysics;
using TGC.Core.Fog;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        public ColisionesTerreno Colisiones;

        //Objetos especiales
        private Barco Barco;
        private MesaCrafteo Mesa;
        private List<IRenderable> Objetos;

        private Suelo Suelo;
        private SkyBox Skybox;
        public Superficie Superficie;
        private Sol Sol;
        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Centro.Y;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;
        public float AlturaMar => 18000f;

        public static Mapa Instancia { get; } = new Mapa();

        private Mapa() {
            Objetos = new List<IRenderable>();
        }
              
        public void CargarSkybox() {
            Centro = Suelo.Center;
            Skybox = new SkyBox(Centro);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Sol = new Sol(Centro + new TGCVector3(0, 50000, 0));
        }

        public void CargarTerreno() {
            Suelo = new Suelo();
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(Suelo.GetData());
        }

        public void CargarSuperficie() {
            Superficie = new Superficie();
            Superficie.CargarTerrains();
        }

        public void CargarParedes(TgcScene paredes) {
            foreach (var face in paredes.Meshes) {
                var size = face.BoundingBox.calculateSize();
                var position = face.BoundingBox.Position;
                var body = BulletRigidBodyFactory.Instance.CreateBox(size, 0, position, 0, 0, 0, 1, false);
                AgregarBody(body);
            }
            paredes.DisposeAll();
        }

        public void CargarPalmeras(TgcScene palmeras) {
            foreach (var palmera in palmeras.Meshes) {
                Objetos.Add(new Palmera(palmera));
                palmera.AlphaBlendEnable = true;
            }
        }

        public void CargarRocas(TgcScene rocas) {
            foreach (var roca in rocas.Meshes) {
                Objetos.Add(new Roca(roca));
            }
        }

        public void CargarExtras(TgcScene extras) {
            foreach (var objeto in extras.Meshes)
                Objetos.Add(new Extra(objeto));
            Barco = new Barco();
            Mesa = new MesaCrafteo();
            Objetos.Add(Barco);
            Objetos.Add(Mesa);
            CambiarEfecto(false);
        }

        public void Update(float elapsedTime, GameModel game) {
            if (game.Player.Posicion.Y > Superficie.Altura) {
                Colisiones.CambiarGravedad(Constants.StandardGravity);
            } else {
                Colisiones.CambiarGravedad(Constants.UnderWaterGravity);
            }
            Objetos.ForEach(o => o.Update(game));
            Suelo.Update(elapsedTime, game.Player.CamaraInterna.PositionEye);
            Superficie.Update(game);
            Sol.Update(elapsedTime);
            Colisiones.Update(elapsedTime);

            Efectos.ActualizarNiebla();
        }

        public void CambiarEfecto(bool sumergido) {
            var efecto = sumergido ? Efectos.EfectoLuzNiebla : Efectos.EfectoLuz;
            var technique = sumergido ? "Nublado" : "Iluminado";
            Objetos.ForEach(o => {
                o.Mesh.Effect = efecto;
                o.Mesh.Technique = technique;
            });
            Suelo.CambiarEfecto(efecto, technique);
            Skybox.CambiarEfecto(efecto, technique);
            Superficie.CambiarEfecto(efecto, technique);
        }

        public void AgregarBody(RigidBody body) {
            Colisiones.AgregarBody(body);
        }

        public void SacarBody(RigidBody body) {
            Colisiones.SacarBody(body);
        }

        public bool Colisionan(CollisionObject ob1, CollisionObject ob2) {
            return Colisiones.Colisionan(ob1, ob2);
        }

        public void Render(Player player) {
            Suelo.Render();
            Skybox.Render();
            Objetos.ForEach(o => o.Render());
            Superficie.Render();
            Sol.Render();
        }

        public void Dispose() {
            Suelo.Dispose();
            Skybox.Dispose();
            Barco.Dispose();
            Objetos.ForEach(o => o.Dispose());
            Superficie.Dispose();
            Sol.Dispose();
            MeshLoader.Instance.Dispose();
        }

    }
}