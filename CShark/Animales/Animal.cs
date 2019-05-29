using BulletSharp;
using BulletSharp.Math;
using CShark.Model;
using CShark.Terreno;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Animales
{
    public abstract class Animal : IAnimal
    {
        protected IComportamiento Comportamiento;
        protected bool Vivo = true;
        protected float Vida;        
        protected bool UsarTransformacionFisica;
        public TgcMesh Mesh;
        public RigidBody Body;

        public Animal(string mesh, TGCVector3 posicionInicial) {
            var ruta = Game.Default.MediaDirectory + @"Animales\" + mesh + "-TgcScene.xml";
            Mesh = new CargadorEscena().loadSceneFromFile(ruta).Meshes[0];
            Posicion = posicionInicial;
            UsarTransformacionFisica = false;
            Mesh.AutoTransformEnable = false;            
        }

        public virtual void Update(GameModel game) {
            if (!UsarTransformacionFisica)
                Body.WorldTransform = TGCMatrix.Translation(Posicion).ToBsMatrix;
            Comportamiento.Update(game.ElapsedTime, this);
        }

        public virtual void Render() {
            Mesh.Transform = UsarTransformacionFisica
                ? new TGCMatrix(Body.InterpolationWorldTransform)
                : ArmarTransformacion();
            Mesh.Render();
            Comportamiento.Render();
        }

        public void Morir() {
            Vivo = false;
            UsarTransformacionFisica = true;
            Mapa.Instancia.SacarBody(Body);
            Body.SetMassProps(500f, Vector3.Zero);
            Mapa.Instancia.AgregarBody(Body);
        }

        public TGCMatrix ArmarTransformacion() {
            return TGCMatrix.Scaling(Escala) *
                TGCMatrix.RotationYawPitchRoll(Rotacion.Y, Rotacion.X, Rotacion.Z) 
                * TGCMatrix.Translation(Posicion);
        }

        public void Dispose() {
            Mesh.Dispose();
        }

        public TGCVector3 Posicion {
            get { return Mesh.Position; }
            set { Mesh.Position = value; }
        }

        public TGCVector3 Rotacion {
            get { return Mesh.Rotation; }
            set { Mesh.Rotation = value; }
        }

        public TGCVector3 Escala;
    }
}
