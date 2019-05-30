using System;
using System.Collections.Generic;
using System.Linq;
using CShark.Model;
using CShark.Variables;
using static CShark.Model.GameModel;

namespace CShark.UI
{
    public class MenuOpciones : Menu
    {
        private List<Checkbox> Checkboxes;
        private int Separacion = 70;
        private Boton BotonVolver;

        public MenuOpciones() : base() {
            Checkboxes = new List<Checkbox>();
        }

        public override void Update(GameModel juego) {
            Checkboxes.ForEach(c => c.Update(juego.Input));
            BotonVolver.Update(juego);
        }

        public override void Render() {
            base.Render();
            Checkboxes.ForEach(c => c.Render());
            BotonVolver.Render();
        }

        public void AgregarCheckbox(Variable<bool> variable) {
            var posicionX = DeviceWidth / 2 + DeviceWidth / 8 - 25;
            var posicionY = Checkboxes.Count() > 0 ? Checkboxes.Last().Posicion.Y + Separacion : (int)RightMenuPos_Y + Separacion + 15;
            Checkboxes.Add(new Checkbox(variable, posicionX, posicionY));
        }
        public void AgregarBoton(string texto, Action<GameModel> accion)
        {
            var posicionX = DeviceWidth / 2 + DeviceWidth / 4;
            var posicionY = Checkboxes.Last().Posicion.Y  + Separacion - 25;
            BotonVolver = new Boton(texto, posicionX, posicionY, accion);
        }

    }
}
