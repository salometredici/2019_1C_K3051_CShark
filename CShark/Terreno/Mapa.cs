using System;
using System.Collections.Generic;
using System.Drawing;
using BulletSharp;
using CShark.EfectosLuces;
using CShark.Fisica.Colisiones;
using CShark.Jugador;
using CShark.Model;
using CShark.Objetos;
using CShark.Optimizaciones;
using CShark.Utilidades;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;
using TGC.Core.Direct3D;
using TGC.Core.Fog;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        public ColisionesTerreno Colisiones;
        private Octree Octree;

        //Objetos especiales
        private Barco Barco;
        private MesaCrafteo Mesa;
        private List<IRenderable> Objetos;

        private Suelo Suelo;
        private SkyBox Skybox;
        public Superficie Superficie;
        private Burbujeador Burbujeador;
        public Sol Sol;
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
            Colisiones.Init(Suelo.Terreno);
        }

        public void CargarSuperficie() {
            Superficie = new Superficie();
            Superficie.CargarTerrains();
        }

        public void CargarParedes(TgcScene paredes) {
            foreach (var face in paredes.Meshes) {
                var size = face.BoundingBox.calculateSize();
                var position = face.Position;
                var body = BulletRigidBodyFactory.Instance.CreateBox(size, 0, position, 0, 0, 0, 1, false);
                AgregarBody(body);
            }
            paredes.DisposeAll();
        }

        public RigidBody CreateRBFromMesh(TgcMesh mesh)
        {
            return BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(mesh);
        }

        public void CargarPalmeras(TgcScene palmeras) {
            foreach (var palmera in palmeras.Meshes) {
                Objetos.Add(new Palmera(palmera));
                palmera.AlphaBlendEnable = true;
                AgregarBody(CreateRBFromMesh(palmera));
            }
        }

        public void CargarRocas(TgcScene rocas) {
            foreach (var roca in rocas.Meshes) {
                Objetos.Add(new Roca(roca));
                AgregarBody(CreateRBFromMesh(roca));
            }
        }

        public void CargarExtras(TgcScene extras) {
            foreach (var objeto in extras.Meshes)
            {
                Objetos.Add(new Extra(objeto));
                AgregarBody(CreateRBFromMesh(objeto));
            }
        }


        public void CargarMesaBarco(TgcMesh mesa, TgcMesh barco) {
            Barco = new Barco(barco);
            Mesa = new MesaCrafteo(mesa);
            Objetos.Add(Barco);
            Objetos.Add(Mesa);
        }

        public void CargarCorales(TgcScene corales) {
            foreach (var coral in corales.Meshes) {
                Objetos.Add(new Coral(coral));
                AgregarBody(CreateRBFromMesh(coral));
            }
        }

        public void Update(float elapsedTime, GameModel game) {
            if (game.Player.Posicion.Y > Superficie.Altura) {
                Colisiones.CambiarGravedad(Constants.StandardGravity);
            } else {
                Colisiones.CambiarGravedad(Constants.UnderWaterGravity);
            }
            Objetos.ForEach(o => o.Update(game));
            Suelo.Update(elapsedTime);
            Superficie.Update(game);
            Burbujeador.Update(game.Player, elapsedTime);
            Sol.Update(elapsedTime);
            Colisiones.Update(elapsedTime);
            Mesa.Update(game);
            ContenedorLuces.Instancia.Update(game.Player.Posicion);
            Efectos.Instancia.ActualizarNiebla();
        }

        public void CargarBurbujas(TGCVector3 spawnPlayer) {
            Burbujeador = new Burbujeador(spawnPlayer, AlturaMar);
        }

        public void CambiarEfecto(bool sumergido) {
            Efectos.Instancia.distanciaNiebla = sumergido ? 30000 : 40000;
            Efectos.Instancia.colorNiebla = sumergido ? Color.Black : Color.LightGray;
            var distanciaFarPlane = Efectos.Instancia.distanciaNiebla + 1000;
            D3DDevice.Instance.Device.Transform.Projection = 
                TGCMatrix.PerspectiveFovLH(FastMath.QUARTER_PI, D3DDevice.Instance.AspectRatio, 
                D3DDevice.Instance.ZNearPlaneDistance, distanciaFarPlane);
            D3DDevice.Instance.ZFarPlaneDistance = distanciaFarPlane;
            Objetos.ForEach(o => {
                o.Mesh.Effect = Efectos.Instancia.EfectoLuzNiebla;
                o.Mesh.Technique = "NubladoIluminado";
            });
            Suelo.CambiarEfecto(Efectos.Instancia.EfectoLuzNiebla, "NubladoIluminado");
            Skybox.CambiarEfecto(Efectos.Instancia.EfectoLuzNiebla, "NubladoIluminado");
            Superficie.CambiarEfecto(Efectos.Instancia.EfectoLuzNiebla, "NubladoIluminado");
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

        public void Render(GameModel game) {
            var FrustumMatrix = TGCMatrix.PerspectiveFovLH(FastMath.QUARTER_PI, D3DDevice.Instance.AspectRatio,
                D3DDevice.Instance.ZNearPlaneDistance, Efectos.Instancia.distanciaNiebla + 1000);
            game.Frustum.updateVolume(TGCMatrix.FromMatrix(D3DDevice.Instance.Device.Transform.View), TGCMatrix.FromMatrix(FrustumMatrix));
            Mesa.Render(game);
            Suelo.Render(game);
            Skybox.Render(game);
            Octree.render(game);
            Superficie.Render();
            Burbujeador.Render();
            Sol.Render();
        }

        public void Dispose() {
            Suelo.Dispose();
            Skybox.Dispose();
            Barco.Dispose();
            Objetos.ForEach(o => o.Dispose());
            Superficie.Dispose();
            Sol.Dispose();
            Mesa.Dispose();
            MeshLoader.Instance.Dispose();
        }

        public void Optimizar() {
            Octree = new Octree();
            var bb = new TgcBoundingAxisAlignBox(new TGCVector3(XMin, YMin, ZMin), new TGCVector3(XMax, YMax, ZMax));
            Octree.create(Objetos, bb);
        }

    }
}