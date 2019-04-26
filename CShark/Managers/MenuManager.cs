using CShark.Model;
using CShark.UI;
using CShark.UI.HUD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;

namespace CShark.Managers
{
    public class MenuManager : IManager
    {
        private MenuPrincipal MenuPrincipal;
        private MenuOpciones MenuOpciones;
        private MenuVariables MenuVariables;
        public Menu MenuSeleccionado;
        private Puntero Puntero;

        public bool MenuAbierto = false;

        public void Initialize() {
            MenuPrincipal = new MenuPrincipal();
            MenuOpciones = new MenuOpciones();
            MenuVariables = new MenuVariables();
            Puntero = new Puntero();
            MenuSeleccionado = MenuPrincipal;

            MenuPrincipal.AgregarBoton("Opciones", j => j.CambiarMenu(TipoMenu.Opciones));
            MenuPrincipal.AgregarBoton("Variables", j => j.CambiarMenu(TipoMenu.Variables));
            MenuPrincipal.AgregarBoton("Cheats", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Salir", j => j.Salir());

            var viewport = D3DDevice.Instance.Device.Viewport;
            var centroPantalla = new Point(viewport.Width / 2, viewport.Height / 2);
            var config = Configuracion.Instancia;

            MenuOpciones.AgregarCheckbox(new Checkbox(config.ModoDios, centroPantalla.X, 100));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.Niebla, centroPantalla.X, 200));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.PostProcesadoCasco, centroPantalla.X, 300));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.MotionBlur, centroPantalla.X, 400));

            MenuVariables.AgregarSlider(new Slider(config.VelocidadRotacion, 0.05f, 0.2f, centroPantalla.X, 100));
            MenuVariables.AgregarSlider(new Slider(config.VelocidadMovimiento, 200, 1000, centroPantalla.X, 200));
        }

        public void CambiarMenu(TipoMenu tipoMenu) {
            MenuSeleccionado = NuevoMenu(tipoMenu);
        }

        public void SwitchMenu() {
            MenuAbierto = !MenuAbierto;
        }

        private Menu NuevoMenu(TipoMenu tipoMenu) {
            switch (tipoMenu)
            {
                case TipoMenu.Principal:
                    return MenuPrincipal;
                case TipoMenu.Opciones:
                    return MenuOpciones;
                case TipoMenu.Variables:
                    return MenuVariables;
                default:
                    return null;
            }
        }

        public void Render(GameModel game) {
            if (MenuAbierto)
            {
                MenuSeleccionado.Render();
                Puntero.Render();
            }
        }

        public void Update(GameModel game) {
            if (MenuAbierto)
            {
                MenuSeleccionado.Update(game);
                Puntero.Update();
            }
        }
    }
}
