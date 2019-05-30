using System;
using System.Collections.Generic;
using System.Linq;
using CShark.Model;
using CShark.Variables;
using static CShark.Model.GameModel;

namespace CShark.UI
{
    public class MenuVariables : Menu
    {
        private List<Slider> Sliders;
        private readonly int Separacion = 90;
        private Boton BotonVolver;

        public MenuVariables() : base() {
            Sliders = new List<Slider>();
        }

        public override void Update(GameModel juego) {
            Sliders.ForEach(s => s.Update(juego.Input));
            BotonVolver.Update(juego);
        }

        public override void Render() {
            base.Render();
            Sliders.ForEach(s => s.Render());
            BotonVolver.Render();
        }

        public void AgregarSlider(Variable<float> variable, float min, float max) {
            var x = DeviceWidth / 2 + DeviceWidth / 8;
            var y = Sliders.Count() > 0 ? (int)Sliders.Last().Posicion.Y + Separacion : (int)RightMenuPos_Y + Separacion + 15;
            Sliders.Add(new Slider(variable, min, max, x, y));
        }

        public void AgregarBoton(string texto, Action<GameModel> accion)
        {
            var posicionX = DeviceWidth / 2 + DeviceWidth / 4;
            var posicionY = (int)Sliders.Last().Posicion.Y + Separacion - 50;
            BotonVolver = new Boton(texto, posicionX, posicionY, accion);
        }
    }
}
