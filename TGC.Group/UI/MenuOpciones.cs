using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Group.Model;

namespace TGC.Group.UI
{
    public class MenuOpciones : Menu
    {
        private List<Slider> Sliders;

        public MenuOpciones() : base() {
            Sliders = new List<Slider>();
            Sliders.Add(new Slider("asdasd", 50, 100, 500, 500));
        }

        public override void Update(GameModel juego) {
            Sliders.ForEach(s => s.Update(juego.Input));
        }

        public override void Render() {
            base.Render();
            Sliders.ForEach(s => s.Render());
        }
    }
}
