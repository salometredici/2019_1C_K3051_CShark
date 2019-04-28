using CShark.Managers;
using CShark.NPCs.Peces;
using CShark.UI;
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
        }


    }
}
