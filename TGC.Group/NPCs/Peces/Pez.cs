using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.NPCs.Peces
{
    public abstract class Pez : IAnimal
    {
        protected float VelocidadRotacion;
        protected float VelocidadMovimiento;
        protected TgcMesh Mesh;
        public TGCVector3 Tamaño { get; set; }

        protected float Direccion = 1f;
        protected float DireccionRot = 1f;

        public Pez(string mesh, TGCVector3 posicion, float velocidadRotacion, float velocidadMovimiento) {
            var ruta = Game.Default.MediaDirectory + "Animales\\" + mesh + "-TgcScene.xml";
            Mesh = new TgcSceneLoader().loadSceneFromFile(ruta).Meshes[0];
            Mesh.Position = posicion;
            VelocidadRotacion = velocidadRotacion;
            VelocidadMovimiento = velocidadMovimiento;
        }
        
        public void Update(float elapsedTime) {
            Moverse(elapsedTime);
            Aletear(elapsedTime);
        }

        public void Render() {
            Mesh.Render();
        }

        public abstract void Moverse(float elapsedTime);
        public abstract void Aletear(float elapsedTime);
        
        
    }
}
