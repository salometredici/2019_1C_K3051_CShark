using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using CShark.EfectosLuces;
using CShark.Model;
using CShark.Terreno;
using TGC.Core.BoundingVolumes;
using TGC.Core.BulletPhysics;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Objetos
{
    public class Roca : Iluminable
    {
        private RigidBody Body;

        public Roca(TgcMesh mesh) : base(mesh, Materiales.Roca) {
            Body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(Mesh);
            Mapa.Instancia.AgregarBody(Body);
        }
    }
}
