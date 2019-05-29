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

namespace CShark.Managers
{
    public class FaunaManager : IManager
    {
        private List<Animal> Animales;
        public static Tiburon Tiburon;

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
                Spawnear(tipo, posicion);
            }*/
            Spawnear("Tiburon", new TGCVector3(85195,12304,-62625));
        }

        private TgcBoundingAxisAlignBox AreaSpawneable1;
        private TgcBoundingAxisAlignBox AreaSpawneable2;

        private void CalcularAreaSpawneable() {
            var loader = new CargadorEscena();
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

        private void Spawnear(string tipo, TGCVector3 posicion) {
            if (tipo.Contains("Tiburon")) {
                Tiburon = new Tiburon(posicion);
                Animales.Add(Tiburon);
            }
            else if (tipo.Contains("Payaso"))
                Animales.Add(new PezPayaso(posicion));
            else if (tipo.Contains("Azul"))
                Animales.Add(new PezAzul(posicion));
            else if (tipo.Contains("Betta"))
                Animales.Add(new PezBetta(posicion));
            else if (tipo.Contains("Tropical")) {
                var tipoPez = new Random().Next(1, 6);
                Animales.Add(new PezTropical(tipoPez, posicion));
            }
        }

        public void Update(GameModel game) {
            Animales.ForEach(animal => animal.Update(game));
        }

        public void Render(GameModel game) {
            Animales.ForEach(animal => animal.Render());
        }

        public void Dispose() {
            Animales.ForEach(a => a.Dispose());
            Tiburon.Dispose();
        }
    }
}
