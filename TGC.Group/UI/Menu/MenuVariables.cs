using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.UI
{
    public class MenuVariables : Menu
    {
        private List<Slider> Sliders;

        public MenuVariables() : base() {
            Sliders = new List<Slider>();
        }

        public override void Update(GameModel juego) {
            Sliders.ForEach(s => s.Update(juego.Input));
        }

        public override void Render() {
            base.Render();
            Sliders.ForEach(s => s.Render());
        }

        public void AgregarSlider(Slider slider) {
            Sliders.Add(slider);
        }
    }
}
