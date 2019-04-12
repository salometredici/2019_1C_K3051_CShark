using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Group.Model;

namespace TGC.Group.UI
{
    public class MenuOpciones : Menu
    {
        private List<Checkbox> Checkboxes;

        public MenuOpciones() : base() {
            Checkboxes = new List<Checkbox>();
        }

        public override void Update(GameModel juego) {
            Checkboxes.ForEach(c => c.Update(juego.Input));
        }

        public override void Render() {
            base.Render();
            Checkboxes.ForEach(c => c.Render());
        }

        public void AgregarCheckbox(Checkbox checkbox) {
            Checkboxes.Add(checkbox);
        }
    }
}
