using BulletSharp;
using CShark.Fisica;
using CShark.Jugador;
using CShark.Luces;
using CShark.Luces.Materiales;
using CShark.Model;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Terreno
{
    public class Barco : IDisposable
    {
        private TgcMesh Mesh;
        private MesaCrafteo Mesa;
        private RigidBody Body;
        private Effect Efecto;
        private IMaterial Material;

        //referencia porque esta dentro de su constructor..
        public Barco(TgcMesh mesh) {
            var loader = new TgcSceneLoader();
            var mesa = loader.loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Mesa-TgcScene.xml").Meshes[0];
            Mesh = mesh;
            Mesa = new MesaCrafteo(mesa);
            var size = Mesh.BoundingBox.calculateSize() - new TGCVector3(0, 250, 0);
            var pos = Mesh.BoundingBox.Position;
            Body = BulletRigidBodyFactory.Instance.CreateBox(new TGCVector3(size.X,size.Y -20f, size.Z), 0, pos, 0, 0, 0, 0.5f, false);
            var mesaRB = BulletRigidBodyFactory.Instance.CreateBox(Mesa.Size, 1f, Mesa.Position, 0, 0, 0, 0.5f, false);
            Mapa.Instancia.AgregarBody(Body);
            Mapa.Instancia.AgregarBody(mesaRB);
            Efecto = Iluminacion.EfectoLuz;
            Mesh.Effect = Efecto;
            Mesh.Technique = "Iluminado";
            Material = new Metal();
        }
        
        public void Update(GameModel game) {
            Iluminacion.ActualizarEfecto(Efecto, Material, game.Camara.Position);
            Mesa.Update(game);
        }

        public void Render() {
            Mesh.Render();
            Mesa.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
            Mesa.Dispose();
        }
    }
}
