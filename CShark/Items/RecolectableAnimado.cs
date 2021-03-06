﻿using CShark.EfectosLuces;
using CShark.Model;
using CShark.Objetos;
using CShark.Utilidades;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Interpolation;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using Material = CShark.Objetos.Material;

namespace CShark.Items
{
    public abstract class RecolectableAnimado : Recolectable
    {
        private TgcMesh _mesh;
        private float rotacion;
        private TGCVector3 posicion; //estatica central
        private TGCVector3 escala;
        private float offsetLetra;
        private InterpoladorVaiven Interpolador;
        private TgcMesh LetraE1;
        private TgcMesh LetraE2;
        private TGCVector3 escalaBox; //un poco mas grande para agarre

        public override TgcBoundingAxisAlignBox BoundingBox => _box;

        public override TgcMesh Mesh => _mesh;
        public override TGCVector3 Posicion => posicion;
        public override TGCVector3 Rotacion => new TGCVector3(0,rotacion,0);

        private TgcBoundingAxisAlignBox _box;

        private Effect Efecto;
        public override Material Material => Materiales.Metal;
        private Luz Luz;

        public RecolectableAnimado(string mesh, float _escala, TGCVector3 _posicion, float _offsetLetra, Color colorLuz) : base(_posicion) {
            _mesh = MeshLoader.GetInstance(mesh);
            LetraE1 = MeshLoader.GetInstance("LetraE");
            LetraE2 = MeshLoader.GetInstance("LetraE");
            LetraE1.AutoTransformEnable = false;
            _mesh.AutoTransformEnable = false;
            LetraE2.AutoTransformEnable = false;
            escala = new TGCVector3(_escala, _escala, _escala);
            rotacion = 0f;
            posicion = _posicion;
            offsetLetra = _offsetLetra;
            escalaBox = new TGCVector3(1.3f, 1.3f, 1.3f);
            Interpolador = new InterpoladorVaiven {
                Min = -30f,
                Max = 30f,
                Current = 0,
                Speed = 135f
            };
            _box = GenerarBox();
            _box.transform(TGCMatrix.Scaling(escala) * TGCMatrix.Translation(posicion));
            Efecto = Efectos.Instancia.EfectoLuzNiebla;
            Efecto.Technique = "Nublado";
            Luz = new Luz(colorLuz, posicion, 10f, 0.1f);
            Efectos.Instancia.AgregarLuz(Luz);
        }

        private TGCMatrix GetLetraTransform(float offsetX, float rot) {
            var yRot = TGCMatrix.RotationY(FastMath.PI / 2 + rot);
            var offset = TGCMatrix.Translation(new TGCVector3(offsetX, 0, 0));
            var orbita = TGCMatrix.RotationY(rotacion);
            var reposicion = TGCMatrix.Translation(posicion);
            return yRot * offset * orbita * reposicion;
        }

        private TGCMatrix GetMeshTransform(float elapsedTime) {
            var yRot = TGCMatrix.RotationY(rotacion);
            var scale = TGCMatrix.Scaling(escala);
            var pos = TGCMatrix.Translation(posicion + new TGCVector3(0, Interpolador.update(elapsedTime), 0));
            return yRot * scale * pos;
        }

        private TgcBoundingAxisAlignBox GenerarBox() {
            var size = Mesh.BoundingBox.calculateSize();
            var lado = size.X > size.Y ? size.X : size.Y;
            lado = size.Z > lado ? size.Z : lado;
            var centro = Mesh.BoundingBox.calculateBoxCenter();
            var m = lado; //escalar si quiero..
            var pmin = new TGCVector3(centro.X - m, centro.Y - m, centro.Z - m);
            var pmax = new TGCVector3(centro.X + m, centro.Y + m, centro.Z + m);
            return new TgcBoundingAxisAlignBox(pmin, pmax);
        }

        public override void Render(GameModel game) {
            if (!Recogido) {
                rotacion += game.ElapsedTime * 2f;
                Mesh.Transform = GetMeshTransform(game.ElapsedTime);
                LetraE1.Transform = GetLetraTransform(offsetLetra, 0);
                LetraE2.Transform = GetLetraTransform(-offsetLetra, FastMath.PI);
                Mesh.Render();
                if (PuedeRecoger(game.Player)) {
                    LetraE1.Render();
                    LetraE2.Render();
                }
            }
        }


        public override void Dispose() {
            Mesh.Dispose();
            LetraE1.Dispose();
            LetraE2.Dispose();
        }

        public override void Desaparecer() {
            Efectos.Instancia.SacarLuz(Luz);
        }
    }
}
