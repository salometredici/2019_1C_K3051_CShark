using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using CShark.NPCs.Peces;
using CShark.Model;
using CShark.NPCs.Enemigos;
using TGC.Core.Mathematica;
using CShark.Animales;
using TGC.Core.BoundingVolumes;
using CShark.Terreno;
using CShark.Utilidades;

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        //private List<Animal> Animales;

        public FaunaManager() {
        }

        private void CargarMeshes() {
            var loader = new TgcSceneLoader();
            var ruta = Game.Default.MediaDirectory + @"Animales\";
            var mesh1 = loader.loadSceneFromFile(ruta + "Pez Payaso-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh1, "Pez Payaso");
            var mesh2 = loader.loadSceneFromFile(ruta + "Pez Azul-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh2, "Pez Azul");
            var mesh3 = loader.loadSceneFromFile(ruta + "Pez Betta-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh3, "Pez Betta");
            var mesh4 = loader.loadSceneFromFile(ruta + "Pez Tropical 1-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh4, "Pez Tropical 1");
            var mesh5 = loader.loadSceneFromFile(ruta + "Pez Tropical 2-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh5, "Pez Tropical 2");
            var mesh6 = loader.loadSceneFromFile(ruta + "Pez Tropical 3-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh6, "Pez Tropical 3");
            var mesh7 = loader.loadSceneFromFile(ruta + "Pez Tropical 4-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh7, "Pez Tropical 4");
            var mesh8 = loader.loadSceneFromFile(ruta + "Pez Tropical 5-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh8, "Pez Tropical 5");
            var mesh9 = loader.loadSceneFromFile(ruta + "Pez Tropical 6-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh9, "Pez Tropical 6");
            var mesh10 = loader.loadSceneFromFile(ruta + "Tiburon-TgcScene.xml").Meshes[0];
            MeshLoader.Instance.LoadMesh(mesh10, "Tiburon");
        }

        public void Initialize() {
            //Animales = new List<Animal>();
            CargarMeshes();
            CalcularAreaSpawneable();
            var tipos = new string[] { "Payaso", "Azul", "Betta", "Tropical"};
            var rnd = new Random();
            for (int i = 0; i < 280; i++) {
                var tipo = tipos[rnd.Next(0, tipos.Length)];
                var posicion = SpawnPezRandom(rnd);
                Spawnear(tipo, posicion, rnd);
            }
            for (int i = 0; i < 20; i++) {
                var posicion = SpawnPezRandom(rnd);
                Spawnear("Tiburon", posicion, rnd);
            }
        }

        private TgcBoundingAxisAlignBox AreaSpawneable1;
        private TgcBoundingAxisAlignBox AreaSpawneable2;

        private void CalcularAreaSpawneable() {
            var loader = new TgcSceneLoader();
            var path = Game.Default.MediaDirectory + @"Mapa\SpawnPeces-TgcScene.xml";
            var boxes = loader.loadSceneFromFile(path);
            AreaSpawneable1 = boxes.Meshes[0].BoundingBox;
            AreaSpawneable2 = boxes.Meshes[1].BoundingBox;
        }

        private TGCVector3 SpawnPezRandom(Random random) {
            var area = random.Next(0, 1) == 0 ? AreaSpawneable1 : AreaSpawneable2;
            var x = random.Next((int)area.PMin.X, (int)area.PMax.X);
            var y = random.Next((int)area.PMin.Y, (int)area.PMax.Y);
            var z = random.Next((int)area.PMin.Z, (int)area.PMax.Z);
            return new TGCVector3(x, y, z);
        }

        private void Spawnear(string tipo, TGCVector3 posicion, Random rnd) {
            Animal ani;
            if (tipo.Contains("Tiburon")) {
                var tibu = new Tiburon(posicion);
                ani = tibu;
                Mapa.Instancia.Tiburones.Add(tibu);
            }
            else if (tipo.Contains("Payaso"))
                ani = new PezPayaso(posicion);
            else if (tipo.Contains("Azul"))
               ani = new PezAzul(posicion);
            else if (tipo.Contains("Betta"))
                ani = new PezBetta(posicion);
            else {
                var tipoPez = rnd.Next(1, 6);
               ani = new PezTropical(tipoPez, posicion);
            }
            //Animales.Add(ani);
            Mapa.Instancia.Objetos.Add(ani);
        }

        public void Update(GameModel game) {
            //Animales.ForEach(animal => animal.Update(game));
        }

        public void Render(GameModel game) {

        }

        public void Dispose() {
            //Animales.ForEach(a => a.Dispose());
        }
    }
}
