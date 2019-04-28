using CShark.Managers;
using CShark.NPCs.Peces;
using CShark.UI;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Model
{
    public class GameManager : IManager{

        private List<IManager> Managers;

        private MenuManager MenuManager;
        private MusicPlayer MusicPlayer;
        private FaunaManager PezManager;
        private RecolectablesManager RecolectablesManager;

        public GameManager() {
            Initialize();
        }

        public void Render(GameModel game) {
            Managers.ForEach(m => m.Render(game));
        }

        public void Update(GameModel game) {
            Managers.ForEach(m => m.Update(game));
        }
        
        public void Initialize() {
            Managers = new List<IManager>();
            MenuManager = new MenuManager();
            MusicPlayer = new MusicPlayer();
            PezManager = new FaunaManager();
            RecolectablesManager = new RecolectablesManager();
            Managers.Add(PezManager);
            Managers.Add(MenuManager);
            Managers.Add(RecolectablesManager);
            Managers.ForEach(m => m.Initialize());
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            MenuManager.CambiarMenu(tipoMenu);
        }

        public void SwitchMenu() {
            MenuManager.SwitchMenu();
            MusicPlayer.SwitchMusic(MenuManager.MenuAbierto);
        }

        public void Dispose()
        {
            MusicPlayer.Dispose();
        }

    }
}
