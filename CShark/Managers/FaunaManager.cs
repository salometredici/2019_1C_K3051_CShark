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

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        private List<Animal> Animales;

        public FaunaManager() {
        }

        public void Initialize() {
            Animales = new List<Animal>();
            CalcularAreaSpawneable();
            var tipos = new string[] { "Payaso", "Azul", "Betta", "Tropical" };
            var rnd = new Random();
            /*for (int i = 0; i < 50; i++) {
                var tipo = tipos[rnd.Next(tipos.Length)];
                var posicion = SpawnPezRandom();
                Spawnear(tipo, posicion, rnd);
            }*/
            Spawnear("SpawnTiburon", new TGCVector3(110000f,15000f,-80000f), rnd);// Posicion muy cerca del player es (115000.0f,20500.0f,-91000.0f));
            Spawnear("SpawnTiburon", new TGCVector3(110000f, 10000f, -100000f), rnd);
            Spawnear("SpawnTiburon", new TGCVector3(80000f, 7000f, -20000f), rnd);
            Spawnear("SpawnTiburon", new TGCVector3(85000f, 15000f, -23000f), rnd);
            Spawnear("SpawnTiburon", new TGCVector3(70000f, 12000f, -250000f), rnd);
            Spawnear("SpawnTiburon", new TGCVector3(77000f, 1000f, -100000f), rnd);
            Spawnear("SpawnTiburon", new TGCVector3(60000f, 3000f, -100000f), rnd);
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

        private TGCVector3 SpawnPezRandom() {
            var random = new Random();
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
            else /*(tipo.Contains("Tropical")) */{
                var tipoPez = rnd.Next(1, 6);
               ani = new PezTropical(tipoPez, posicion);
            }
            Animales.Add(ani);
        }

        public void Update(GameModel game) {
            Animales.ForEach(animal => animal.Update(game));
        }

        public void Render(GameModel game) {
            Animales.ForEach(animal => animal.Render(game));
        }

        public void Dispose() {
            Animales.ForEach(a => a.Dispose());
        }
    }
}
