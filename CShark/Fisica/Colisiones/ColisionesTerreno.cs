using BulletSharp;
using BulletSharp.Math;
using CShark.Managers;
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
using TGC.Core.Textures;

namespace CShark.Fisica.Colisiones
{
    public class ColisionesTerreno {

        private DiscreteDynamicsWorld World;
        private CollisionDispatcher Dispatcher;
        private DefaultCollisionConfiguration Configuration;
        private SequentialImpulseConstraintSolver ConstraintSolver;
        private BroadphaseInterface OverlappingPairCache;
        private CustomVertex.PositionTextured[] DataTriangulos;
        
        public void Init(CustomVertex.PositionTextured[] dataTriangulos) {
            DataTriangulos = dataTriangulos;
            Configuration = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(Configuration);
            GImpactCollisionAlgorithm.RegisterAlgorithm(Dispatcher);
            ConstraintSolver = new SequentialImpulseConstraintSolver();
            OverlappingPairCache = new DbvtBroadphase();
            World = new DiscreteDynamicsWorld(Dispatcher, OverlappingPairCache, ConstraintSolver, Configuration);

            World.Gravity = new Vector3(0, -100f, 0);

            //Creamos el terreno
            var bodyTerreno = BulletRigidBodyFactory.Instance.CreateSurfaceFromHeighMap(DataTriangulos);
            World.StepSimulation(1 / Configuracion.Instancia.FPS.Valor, 100);

            World.AddRigidBody(bodyTerreno);
        }

        public void AgregarBody(RigidBody body) {
            World.AddRigidBody(body);
        }

        public void Update() {
            World.StepSimulation(1 / Configuracion.Instancia.FPS.Valor, 100);
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
