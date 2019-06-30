using CShark.Items;
using CShark.Items.Recolectables;
using CShark.Model;
using CShark.Objetos;
using CShark.Terreno;
using CShark.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Managers
{
    public class RecolectablesManager : IManager
    {
        private List<IRecolectable> Recolectables;

        public RecolectablesManager() {
        }

        public void Initialize() {
            Recolectables = new List<IRecolectable>();
            CargarMeshes();
            this.SpawnearSobre(Mapa.Instancia.VerticesSuelo, 400);
            this.SpawnearSobre(Mapa.Instancia.Extras, 100);
            this.SpawnearSobre(Mapa.Instancia.Rocas, 150);
            Mapa.Instancia.VerticesSuelo = null;
            Mapa.Instancia.Extras = null;
            Mapa.Instancia.Rocas = null;
        }

        private void SpawnearSobre(List<IRenderable> objetos, int cantidad) {
            var ingresados = new int[cantidad];
            var random = new Random();
            for (int i = 0; i < Math.Min(cantidad, objetos.Count() - 10); i++) {
                var indice = random.Next(objetos.Count);
                while (contiene(ingresados, indice)) //si no riperino
                    indice = random.Next(objetos.Count);
                ingresados[i] = indice;
                var obj = objetos[indice];
                var vertices = obj.Mesh.getVertexPositions();
                var centro = obj.BoundingBox.calculateBoxCenter();
                var verticeCentro = vertices
                    .OrderBy(v => cercaniaCentro(v, centro))
                    .OrderByDescending(v => v.Y)
                    .First();
                Spawnear(verticeCentro + new TGCVector3(0, 200, 0), random);
            }
        }

        private bool cercaniaCentro(TGCVector3 vertice, TGCVector3 centro) {
            return FastMath.Abs(vertice.X - centro.X) < 50 && FastMath.Abs(vertice.Z - centro.Z) < 50;
        }

        private void SpawnearSobre(TGCVector3[] vertices, int cantidad) {
            var ingresados = new int[cantidad];
            var random = new Random();
            for (int i = 0; i < Math.Min(cantidad, vertices.Length - 10); i++) {
                var indice = random.Next(vertices.Length);
                while (contiene(ingresados, indice))
                    indice = random.Next(vertices.Length);
                ingresados[i] = indice;
                var vert = vertices[indice];
                Spawnear(vert + new TGCVector3(0, 200, 0), random);
            }
        }

        private bool contiene(int[] array, int i) {
            for (int j = 0; j < array.Length; j++)
                if (array[j] == i)
                    return true;
            return false;
        }

        private int rndSg(Random rnd) {
            return rnd.Next(0, 1) == 1 ? 1 : -1;
        }

        private void Spawnear(TGCVector3 posicion, Random random) {
            var item = RandomItem(posicion, random);
            Recolectables.Add(item);
            Mapa.Instancia.Objetos.Add(item);
        }

        private ERecolectable RandomTipo(Random random) {
            Array values = Enum.GetValues(typeof(ERecolectable));
            return (ERecolectable)values.GetValue(random.Next(values.Length));
        }

        private IRecolectable RandomItem(TGCVector3 posicion, Random random) {
            var tipo = RandomTipo(random);
            switch (tipo) {
                case ERecolectable.Pila: return new Pila(posicion);
                case ERecolectable.Burbuja: return new Burbuja(posicion);
                case ERecolectable.Chip: return new Chip(posicion);
                case ERecolectable.Wumpa: return new Wumpa(posicion);
                case ERecolectable.Arpon: return new Arpon(posicion);
                case ERecolectable.Coral: return new Items.Recolectables.Coral(posicion);
                case ERecolectable.Hierro: return new Hierro(posicion);
                case ERecolectable.Medkit: return new Medkit(posicion);
                case ERecolectable.Oro: return new Oro(posicion);
                case ERecolectable.Oxigeno: return new Oxigeno(posicion);
                case ERecolectable.Pez: return new Pez(posicion);
                case ERecolectable.Plata: return new Plata(posicion);
                default: return new Pila(posicion);
            }
        }

        private void CargarMeshes() {
            var path = Game.Default.MediaDirectory + @"Recolectables\";
            var loader = new TgcSceneLoader();
            var meshes = new List<string>() {
                "Arpon", "Chip", "Coral", "Hierro",
                "LetraE", "Medkit", "Oro", "Oxigeno",
                "Pez", "Pila", "Plata", "Wumpa"
            };
            meshes.ForEach(m => MeshLoader.Instance.LoadMesh(path, m));
        }

        public void Update(GameModel game) {
            //Recolectables.ForEach(r => r.Update(game));
        }

        public void Dispose() {
            Recolectables.ForEach(r => r.Dispose());
        }

        public void Render(GameModel game) {

        }
    }
}
