using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Group.Model;
using TGC.Group.Utils;

namespace TGC.Group.UI
{
    public class MenuPrincipal : Menu
    {
        private List<Boton> Botones;
        private int Separacion; //entre botones
        private int AlturaBotones;

        public int CantidadBotones => Botones.Count();

        public MenuPrincipal() : base() {
            Botones = new List<Boton>();
            Separacion = 50;
            AlturaBotones = 75;
            AgregarBoton("Opciones");
            AgregarBoton("Variables");
            AgregarBoton("Cheats");
            AgregarBoton("Salir");
        }

        public void AgregarBoton(string texto) {
            var posicionX = D3DDevice.Instance.Device.Viewport.Width / 2;
            var posicionY = CantidadBotones * (Separacion + AlturaBotones);
            Botones.Add(new Boton(texto, posicionX, posicionY));
        }

        public override void Update(GameModel juego) {
            Botones.ForEach(b => b.Update(juego));
        }

        public override void Render() {
            base.Render();
            Botones.ForEach(b => b.Render());            
        }

    }
}
