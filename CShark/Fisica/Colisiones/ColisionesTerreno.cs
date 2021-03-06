﻿using BulletSharp;
using BulletSharp.Math;
using CShark.Managers;
using CShark.Model;
using CShark.NPCs.Enemigos;
using CShark.Utils;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace CShark.Fisica.Colisiones
{
    public class ColisionesTerreno {

        public DiscreteDynamicsWorld World;
        private CollisionDispatcher Dispatcher;
        private DefaultCollisionConfiguration Configuration;
        private SequentialImpulseConstraintSolver ConstraintSolver;
        private BroadphaseInterface OverlappingPairCache;
        public RigidBody FondoDelMarRB;

        BroadphaseInterface Broadphase;

        public void CambiarGravedad(float gravedad)
        {
            World.Gravity = new Vector3(0, gravedad, 0);
        }

        public void CambiarGravedad(Vector3 gravedad) {
            World.Gravity = gravedad;
        }

        public void Init(TgcMesh terreno) {
            Configuration = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(Configuration);

            Broadphase = new AxisSweep3(new Vector3(-1000, -1000, -1000), new Vector3(1000, 1000, 1000));
            Broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new GhostPairCallback());

            GImpactCollisionAlgorithm.RegisterAlgorithm(Dispatcher);


            ConstraintSolver = new SequentialImpulseConstraintSolver();
            OverlappingPairCache = new DbvtBroadphase();

            World = new DiscreteDynamicsWorld(Dispatcher, OverlappingPairCache, ConstraintSolver, Configuration)
            {
                Gravity = new Vector3(0, -5000f, 0) //Constants.StandardGravity// No sé qué rompí que ahora no me deja usar las keys de Constants
            };

            FondoDelMarRB = BulletRigidBodyFactory.Instance.CreateRigidBodyFromTgcMesh(terreno);
            FondoDelMarRB.Friction = 0f;
            World.AddRigidBody(FondoDelMarRB);
        }

        //OPTIMIZAR ESTA BASURA
        public bool Colisionan(CollisionObject o1, CollisionObject o2) {
            int numManifolds = World.Dispatcher.NumManifolds;
            for (int i = 0; i < numManifolds; i++)
            {
                PersistentManifold contactManifold = World.Dispatcher.GetManifoldByIndexInternal(i);
                
                CollisionObject obA = contactManifold.Body0 as CollisionObject;
                CollisionObject obB = contactManifold.Body1 as CollisionObject;

                if (o1.Equals(obA) && o2.Equals(obB) || o2.Equals(obA) && o1.Equals(obB))
                    return true;
            }
            return false;
        }

        public void AgregarBody(RigidBody body) {
            World.AddRigidBody(body);
        }

        public void SacarBody(RigidBody body) {
            World.RemoveRigidBody(body);
        }

        public void Update(float elapsedTime) {
            World.StepSimulation(elapsedTime);
        }

        public void Dispose() {
            World.Dispose();
            Dispatcher.Dispose();
            Configuration.Dispose();
            ConstraintSolver.Dispose();
            OverlappingPairCache.Dispose();
        }

    }
}
