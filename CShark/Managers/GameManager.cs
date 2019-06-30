using CShark.EfectosLuces;
using CShark.Managers;
using CShark.NPCs.Peces;
using CShark.Objetos;
using CShark.Terreno;
using CShark.UI;
using CShark.Utilidades;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;

namespace CShark.Model
{
    public class GameManager : IManager{

        private List<IManager> Managers;

        private LoadingScreen PantallaCarga;
        private MenuManager MenuManager;
        private MusicPlayer MusicPlayer;
        public TgcDirectSound DirectSound;
        private FaunaManager PezManager;
        private RecolectablesManager RecolectablesManager;

        public TGCVector3 SpawnPlayer;

        public GameManager() {
            PantallaCarga = new LoadingScreen(13);
            var th = new Thread(Initialize);
            th.Start();
            PantallaCarga.Render();
        }

        public void Render(GameModel game) {
            if (!game.Player.onPause)
            {
                Managers.ForEach(m => m.Render(game));
            }
            else
            {
                MenuManager.Render(game);
            }
        }

        public void Update(GameModel game) {
            if (!game.Player.onPause)
            {
                Managers.ForEach(m => m.Update(game));
            }
            else
            {
                MenuManager.Update(game);
            }
        }

        public void Initialize() {
            var loader = new TgcSceneLoader();
            var media = Game.Default.MediaDirectory;
            var mapPath = media + "Mapa";
            PantallaCarga.Progresar("Cargando terreno...");
            Mapa.Instancia.CargarTerreno();
            PantallaCarga.Progresar("Cargando skybox...");
            Mapa.Instancia.CargarSkybox();
            PantallaCarga.Progresar("Cargando superficie...");
            Mapa.Instancia.CargarSuperficie();
            PantallaCarga.Progresar("Cargando palmeras...");
            var palmeras = loader.loadSceneFromFile(media + @"Mapa\Palmeras-TgcScene.xml");
            Mapa.Instancia.CargarPalmeras(palmeras);
            PantallaCarga.Progresar("Cargando rocas...");
            var rocas = loader.loadSceneFromFile(media + @"Mapa\Rocas-TgcScene.xml");
            Mapa.Instancia.CargarRocas(rocas);
            PantallaCarga.Progresar("Cargando extras...");
            TexturasColor.CargarColores();
            var extras1 = loader.loadSceneFromFile(media + @"Mapa\Props 1-TgcScene.xml");
            //var extras2 = loader.loadSceneFromFile(media + @"Mapa\Props 2-TgcScene.xml");
            var extras3 = loader.loadSceneFromFile(media + @"Mapa\Props 3-TgcScene.xml");
            var barco = extras1.getMeshByName("Barco");
            var mesa = extras1.getMeshByName("Mesa");
            extras1.Meshes.Remove(barco);
            extras1.Meshes.Remove(mesa);
            Mapa.Instancia.CargarExtras(extras1);
            //Mapa.Instancia.CargarExtras(extras2);
            Mapa.Instancia.CargarExtras(extras3);
            Mapa.Instancia.CargarMesaBarco(mesa, barco);
            PantallaCarga.Progresar("Cargando corales...");
            Mapa.Instancia.CargarCorales(loader.loadSceneFromFile(media + @"Mapa\Corales 1-TgcScene.xml"));
            Mapa.Instancia.CargarCorales(loader.loadSceneFromFile(media + @"Mapa\Corales 2-TgcScene.xml"));
            Mapa.Instancia.CargarCorales(loader.loadSceneFromFile(media + @"Mapa\Corales 3-TgcScene.xml"));
            Managers = new List<IManager>();
            PantallaCarga.Progresar("Cargando peces...");
            PezManager = new FaunaManager();
            Managers.Add(PezManager);
            PezManager.Initialize();
            SpawnPlayer = barco.BoundingBox.calculateBoxCenter() + new TGCVector3(0, 3000, 0);
            PantallaCarga.Progresar("Cargando menús...");
            MenuManager = new MenuManager();
            Managers.Add(MenuManager);
            MenuManager.Initialize();
            PantallaCarga.Progresar("Cargando recolectables...");
            Mapa.Instancia.CargarBurbujas(SpawnPlayer);
            var recolectables = loader.loadSceneFromFile(media + @"Mapa\Recolectables-TgcScene.xml");
            RecolectablesManager = new RecolectablesManager(recolectables);
            Managers.Add(RecolectablesManager);
            RecolectablesManager.Initialize(recolectables);
            PantallaCarga.Progresar("Optimizando...");
            Mapa.Instancia.Optimizar();
            PantallaCarga.Progresar("Cargando paredes invisibles...");
            var paredes = loader.loadSceneFromFile(media + @"Mapa\Paredes-TgcScene.xml");
           // Mapa.Instancia.CargarParedes(paredes);
            Mapa.Instancia.CambiarEfecto(false);
            PantallaCarga.Progresar("Cargando audio...");
            MusicPlayer = new MusicPlayer();
            //DirectSound = new TgcDirectSound();
            ContenedorLuces.Instancia.ArmarLuces();
            PantallaCarga.Finalizar();
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            MenuManager.CambiarMenu(tipoMenu);
        }

        public void SwitchMenu(GameModel game) {
            MenuManager.SwitchMenu();
            MusicPlayer.SwitchMusic(MenuManager.MenuAbierto, false);
            game.Player.onPause = !game.Player.onPause;
            game.Player.Lock(); //o unlock
        }

        public void Dispose()
        {
            MusicPlayer.Dispose();
            MenuManager.Dispose();
        }

        void IManager.Initialize() {
            throw new NotImplementedException();
        }
    }
}
