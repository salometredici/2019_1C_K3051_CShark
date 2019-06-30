using CShark.Items;
using CShark.Items.Recolectables;
using CShark.Model;
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
        private TgcScene Spawns;

        public RecolectablesManager(TgcScene spawns) {
            Spawns = spawns;
        }

        public void Initialize(TgcScene spawns) {
            Recolectables = new List<IRecolectable>();
            CargarMeshes();
            var random = new Random();
            foreach (var mesh in spawns.Meshes)
                Spawnear(mesh.BoundingBox.Position, random);
            Spawns.DisposeAll();
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
                case ERecolectable.Coral: return new Coral(posicion);
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
            Recolectables.ForEach(r => r.Update(game));
        }

        public void Initialize() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            Recolectables.ForEach(r => r.Dispose());
        }

        public void Render(GameModel game) {

        }
    }
}
