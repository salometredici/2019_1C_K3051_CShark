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
            Initialize();            
        }

        public void Render(GameModel game) {
            Managers.ForEach(m => m.Render(game));
        }

        public void Update(GameModel game) {
            Managers.ForEach(m => m.Update(game));
        }
        
        public async void Initialize() {
            PantallaCarga = new LoadingScreen(9);
            Task task = Task.Run(() => PantallaCarga.Render());
            var loader = new TgcSceneLoader();
            var media = Game.Default.MediaDirectory;
            PantallaCarga.Progresar("Cargando rocas...");
            var rocas = loader.loadSceneFromFile(media + @"Mapa\Rocas-TgcScene.xml");
            PantallaCarga.Progresar("Cargando extras...");
            var extras = loader.loadSceneFromFile(media + @"Mapa\Extras-TgcScene.xml");
            PantallaCarga.Progresar("Cargando spawns...");
            var spawns = loader.loadSceneFromFile(media + @"Mapa\Spawns-TgcScene.xml");
            SpawnPlayer = spawns.getMeshByName("SpawnPlayer").BoundingBox.Position;
            PantallaCarga.Progresar("Posicionando rocas...");
            Mapa.Instancia.CargarRocas(rocas);
            PantallaCarga.Progresar("Posicionando extras...");
            Mapa.Instancia.CargarExtras(extras);
            Managers = new List<IManager>();
            MenuManager = new MenuManager();
            PezManager = new FaunaManager(spawns);
            RecolectablesManager = new RecolectablesManager();
            PantallaCarga.Progresar("Cargando peces...");
            Managers.Add(PezManager);
            PezManager.Initialize();
            PantallaCarga.Progresar("Cargando menús...");
            Managers.Add(MenuManager);
            MenuManager.Initialize();
            PantallaCarga.Progresar("Cargando items...");
            Managers.Add(RecolectablesManager);
            RecolectablesManager.Initialize();
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
