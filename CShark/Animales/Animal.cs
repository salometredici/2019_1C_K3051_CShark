using BulletSharp;
using BulletSharp.Math;
using CShark.Model;
using CShark.Objetos;
using CShark.Terreno;
using CShark.Utilidades;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace CShark.Animales
{
    public abstract class Animal : Iluminable
    {
        protected IComportamiento Comportamiento;
        protected bool Vivo = true;
        protected float Vida;        
        protected bool UsarTransformacionFisica;
        public RigidBody Body;

        public Animal(string mesh, TGCVector3 posicionInicial) :base(MeshLoader.GetInstance(mesh), Materiales.Normal) {
            Posicion = posicionInicial;
            UsarTransformacionFisica = false;        
        }

        public override void Update(GameModel game) {
            base.Update(game);
            if (!UsarTransformacionFisica)
                Body.WorldTransform = TGCMatrix.Translation(Posicion).ToBsMatrix;
            Comportamiento.Update(game.ElapsedTime, this);
        }

        public override void Render(GameModel game) {
            Mesh.Transform = UsarTransformacionFisica
                ? new TGCMatrix(Body.InterpolationWorldTransform)
                : ArmarTransformacion();
            Comportamiento.Render();
            base.Render(game);
        }

        public void Morir() {
            Vivo = false;
            UsarTransformacionFisica = true;
            Mapa.Instancia.SacarBody(Body);
            Body.SetMassProps(500f, Vector3.Zero);
            Mapa.Instancia.AgregarBody(Body);
        }

        public TGCMatrix ArmarTransformacion() {
            return TGCMatrix.Scaling(_escala) *
                TGCMatrix.RotationYawPitchRoll(Rotacion.Y, Rotacion.X, Rotacion.Z) 
                * TGCMatrix.Translation(Posicion);    
        }

        public override void RenderOscuro() {
            var aux = Mesh.Technique;
            Mesh.Technique = "DibujarObjetosOscuros";
            Mesh.Transform = UsarTransformacionFisica
                    ? new TGCMatrix(Body.InterpolationWorldTransform)
                    : ArmarTransformacion();
            Comportamiento.Render();
            Mesh.Technique = aux;
            base.RenderOscuro();
        }

        public TGCVector3 Posicion {
            get { return Mesh.Position; }
            set { Mesh.Position = value; }
        }

        public TGCVector3 Rotacion {
            get { return Mesh.Rotation; }
            set { Mesh.Rotation = value; }
        }

        public float Escala {
            get { return _esc; }
            set {
                _esc = value;
                _escala = new TGCVector3(_esc, _esc, _esc);
            }
        }
        /*
        public Material Material => Materiales.Normal;

        public TgcMesh Mesh => _mesh;

        public TgcBoundingAxisAlignBox BoundingBox => _mesh.BoundingBox;

        public bool Enabled { get; set; }
        */
        private float _esc;
        private TGCVector3 _escala;
    }
}
