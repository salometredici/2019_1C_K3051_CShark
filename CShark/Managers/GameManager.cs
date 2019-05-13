using CShark.Managers;
using CShark.NPCs.Peces;
using CShark.Terreno;
using CShark.UI;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Model
{
    public class GameManager : IManager{

        private List<IManager> Managers;

        private LoadingScreen PantallaCarga;
        private MenuManager MenuManager;
        private MusicPlayer MusicPlayer;
        private FaunaManager PezManager;
        private RecolectablesManager RecolectablesManager;

        public TGCVector3 SpawnPlayer;

        public GameManager() {
            PantallaCarga = new LoadingScreen(10);
            Initialize();
        }

        public void Render(GameModel game) {
            Managers.ForEach(m => m.Render(game));
        }

        public void Update(GameModel game) {
            Managers.ForEach(m => m.Update(game));
        }

        public async void Initialize() {
            PantallaCarga = new LoadingScreen(12);
            Task task = Task.Run(() => PantallaCarga.Render());
            var loader = new TgcSceneLoader();
            var media = Game.Default.MediaDirectory;
            PantallaCarga.Progresar("Cargando terreno...");
            Mapa.Instancia.CargarTerreno();
            PantallaCarga.Progresar("Cargando skybox...");
            Mapa.Instancia.CargarSkybox();
            PantallaCarga.Progresar("Cargando superficie...");
            Mapa.Instancia.CargarSuperficie();
            PantallaCarga.Progresar("Cargando rocas...");
            var rocas = loader.loadSceneFromFile(media + @"Mapa\Rocas-TgcScene.xml");
            PantallaCarga.Progresar("Cargando extras...");
            var extras = loader.loadSceneFromFile(media + @"Mapa\Extras-TgcScene.xml");
            PantallaCarga.Progresar("Cargando spawns...");
            var peces = loader.loadSceneFromFile(media + @"Mapa\Peces-TgcScene.xml");
            //SpawnPlayer = spawns.getMeshByName("SpawnPlayer").BoundingBox.Position;
            SpawnPlayer = new TGCVector3(1500f, 3500f, 0);
            PantallaCarga.Progresar("Posicionando rocas...");
            Mapa.Instancia.CargarRocas(rocas);
            PantallaCarga.Progresar("Posicionando extras...");
            Mapa.Instancia.CargarExtras(extras);

            Managers = new List<IManager>();
            MenuManager = new MenuManager();
            PezManager = new FaunaManager(peces);
            PantallaCarga.Progresar("Cargando peces...");
            Managers.Add(PezManager);
            PezManager.Initialize();
            PantallaCarga.Progresar("Cargando menús...");
            Managers.Add(MenuManager);
            MenuManager.Initialize();
            PantallaCarga.Progresar("Cargando recolectables...");
            var recolectables = loader.loadSceneFromFile(media + @"Mapa\Recolectables-TgcScene.xml");
            RecolectablesManager = new RecolectablesManager(recolectables);
            Managers.Add(RecolectablesManager);
            RecolectablesManager.Initialize(recolectables);
            PantallaCarga.Progresar("Cargando paredes invisibles...");
            var paredes = loader.loadSceneFromFile(media + @"Mapa\Paredes-TgcScene.xml");
            Mapa.Instancia.CargarParedes(paredes);
            PantallaCarga.Progresar("Cargando audio...");
            MusicPlayer = new MusicPlayer();
            await task;
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            MenuManager.CambiarMenu(tipoMenu);
        }

        public void SwitchMenu(GameModel game) {
            MenuManager.SwitchMenu();
            MusicPlayer.SwitchMusic(MenuManager.MenuAbierto);
            game.Player.onPause = !game.Player.onPause;
            game.Player.Lock(); //o unlock
        }

        public void Dispose()
        {
            MusicPlayer.Dispose();
        }

    }
}
