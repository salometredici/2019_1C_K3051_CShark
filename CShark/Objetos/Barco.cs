using BulletSharp;
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
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Barco : Iluminable
    {
        private RigidBody Body;

        public Barco(TgcMesh mesh) : base(mesh, Materiales.Metal) {
            Body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(Mesh);
            Mapa.Instancia.AgregarBody(Body);
        }
    }
}
