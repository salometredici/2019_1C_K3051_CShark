using CShark.Model;
using CShark.Terreno;
using CShark.UI;
using CShark.UI.HUD;
using System.Drawing;
using TGC.Core.Direct3D;
using static CShark.Model.GameModel;

namespace CShark.Managers
{
    public class MenuManager : IManager
    {
        private MenuPrincipal MenuPrincipal;
        private MenuOpciones MenuOpciones;
        private MenuVariables MenuVariables;
        private MenuInventario MenuInventario;
        private MenuCrafteo MenuCrafteo;
        private GuideMenu MenuGuia;
        public Menu MenuSeleccionado;
        private Puntero Puntero;

        public bool MenuAbierto = false;

        public void Initialize() {

            MenuPrincipal = new MenuPrincipal();
            MenuOpciones = new MenuOpciones();
            MenuVariables = new MenuVariables();
            MenuInventario = new MenuInventario("Menu\\Inventario\\");
            MenuCrafteo = new MenuCrafteo("MenuCrafteo\\");
            MenuGuia = new GuideMenu();
            Puntero = new Puntero();

            MenuSeleccionado = MenuPrincipal;

            MenuPrincipal.AgregarBoton("Controles", j => j.CambiarMenu(TipoMenu.Guia));
            MenuPrincipal.AgregarBoton("Opciones", j => j.CambiarMenu(TipoMenu.Opciones));
            MenuPrincipal.AgregarBoton("Variables", j => j.CambiarMenu(TipoMenu.Variables));
            MenuPrincipal.AgregarBoton("Cheats", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Salir", j => j.Salir());
            
            var config = Configuracion.Instancia;

            MenuOpciones.AgregarCheckbox(config.MostrarRayo);
            MenuOpciones.AgregarCheckbox(config.ModoDios);
            MenuOpciones.AgregarCheckbox(config.Niebla);
            MenuOpciones.AgregarCheckbox(config.PostProcesadoCasco);
            MenuOpciones.AgregarCheckbox(config.MotionBlur);
            MenuOpciones.AgregarBoton("Volver", j => j.CambiarMenu(TipoMenu.Principal));

            MenuVariables.AgregarSlider(new Slider(config.VelocidadRotacion, 0.05f, 0.2f, ScreenCenter.X, 100));
            MenuVariables.AgregarSlider(new Slider(config.VelocidadMovimiento, 200, 1000, ScreenCenter.X, 200));
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
                case TipoMenu.Inventario:
                    return MenuInventario;
                case TipoMenu.Crafteo:
                    return MenuCrafteo;
                case TipoMenu.Guia:
                    return MenuGuia;
                case TipoMenu.CombosCrafteo:
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
            else
            {
                var centroPantalla = new Point(DeviceWidth / 2, DeviceHeight / 2);
                System.Windows.Forms.Cursor.Position = centroPantalla;
            }
        }

        public void Dispose() {
            MenuPrincipal.Dispose();
            MenuOpciones.Dispose();
            MenuVariables.Dispose();
            MenuInventario.Dispose();
            MenuCrafteo.Dispose();
            Puntero.Dispose();
        }

    }
}
