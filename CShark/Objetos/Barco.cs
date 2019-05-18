using BulletSharp;
using CShark.Fisica;
using CShark.Jugador;
using CShark.Luces;
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
    public class Barco : IDisposable //cambiar a rendearable
    {
        private TgcMesh Mesh;
        private RigidBody Body;
        private Effect Efecto;
        private Material Material;

        public Barco() {
            Mesh = new TgcSceneLoader().loadSceneFromFile(Game.Default.MediaDirectory + @"Mapa\Barco-TgcScene.xml").Meshes[0];
            Body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(Mesh);
            Mapa.Instancia.AgregarBody(Body);
            Efecto = Iluminacion.EfectoLuz;
            Mesh.Effect = Efecto;
            Mesh.Technique = "Iluminado";
            Material = Materiales.Metal;
        }
        
        public void Update(GameModel game) {
            Iluminacion.ActualizarEfecto(Efecto, Material, game.Camara.Position);
        }

        public void Render() {
            Mesh.Render();
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public void Render(TGCVector3 camara) {
            throw new NotImplementedException();
        }
    }
}
