using BulletSharp;
using CShark.EfectosLuces;
using CShark.Model;
using CShark.Terreno;
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
    public class Palmera : Iluminable
    {
        private RigidBody Body;

        public Palmera(TgcMesh mesh) : base(mesh, Materiales.Madera) {
            Body = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(mesh);
            Mapa.Instancia.AgregarBody(Body);
        }
    }
}
