using CShark.Jugador;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Terreno.Burbujas
{
    public class BurbujaAleatoria : IDisposable
    {
        private List<TgcMesh> Burbujas;
        private TGCVector3 Centro;
        private float Recorrido = 0;
        private const float Velocidad = 1000f;
        private float Limite;
        private Burbujeador Burbujueador;

        public BurbujaAleatoria(float x, float y, float z, TgcMesh mesh, Effect effect, float limite, Burbujeador burbi) {
            Burbujas = new List<TgcMesh>();
            Centro = new TGCVector3(x, y, z);
            Limite = limite;
            var random = new Random();
            Burbujueador = burbi;
            for (int i = 0; i < 6; i++) {
                var burbuja = mesh.createMeshInstance("burbi");
                var s = 100.0f / random.Next(50, 150);
                burbuja.Position = Centro + OffsetAleatorio(random);
                burbuja.Scale = new TGCVector3(s, s, s);
                burbuja.Effect = effect;
                burbuja.Technique = "BurbujaAleatoria";
                burbuja.AlphaBlendEnable = true;
                Burbujas.Add(burbuja);
            }
        }

        public void Update(float elapsedTime) {
            if (Recorrido < 3000 && Centro.Y < Limite) {
                var desplazamiento = elapsedTime * Velocidad;
                Recorrido += desplazamiento;
                Burbujas.ForEach(b => b.Position += new TGCVector3(0, desplazamiento, 0));
            } else {
                Burbujueador.Reemplazar(this);
            }
        }

        public void Render(float time) {
            Burbujas.ForEach(b => {
                b.Effect.SetValue("time", time);
                b.Transform = TGCMatrix.Scaling(b.Scale) * TGCMatrix.Translation(b.Position);
                b.Render();
            });
        }

        public void RenderOscuro(float time) {
            Burbujas.ForEach(b => {
                b.Effect.SetValue("time", time);
                b.Transform = TGCMatrix.Scaling(b.Scale) * TGCMatrix.Translation(b.Position);
                b.Render();
            });
        }

        private TGCVector3 OffsetAleatorio(Random rand) {
            var x = rand.Next(150) * SR(rand);
            var y = rand.Next(300) * SR(rand);
            var z = rand.Next(150) * SR(rand);
            return new TGCVector3(x, y, z);
        }

        //Signo random
        private int SR(Random random) {
            return random.Next(0, 1) == 1 ? 1 : -1;
        }

        public void Dispose() {
            Burbujas.ForEach(b => b.Dispose());
        }
    }
}
