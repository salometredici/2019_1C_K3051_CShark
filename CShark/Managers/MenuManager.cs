using CShark.Model;
using CShark.UI;
using CShark.UI.HUD;
using CShark.UI.Inventario;
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
        public Menu MenuSeleccionado;
        private Puntero Puntero;

        public bool MenuAbierto = false;

        public void Initialize() {

            MenuPrincipal = new MenuPrincipal();
            MenuOpciones = new MenuOpciones();
            MenuVariables = new MenuVariables();
            MenuInventario = new MenuInventario();
            Puntero = new Puntero();

            MenuSeleccionado = MenuPrincipal;

            MenuPrincipal.AgregarBoton("Inventario", j => j.CambiarMenu(TipoMenu.Inventario));
            MenuPrincipal.AgregarBoton("Opciones", j => j.CambiarMenu(TipoMenu.Opciones));
            MenuPrincipal.AgregarBoton("Variables", j => j.CambiarMenu(TipoMenu.Variables));
            MenuPrincipal.AgregarBoton("Cheats", j => j.CambiarMenu(TipoMenu.Principal));
            MenuPrincipal.AgregarBoton("Salir", j => j.Salir());
            
            var config = Configuracion.Instancia;

            MenuOpciones.AgregarCheckbox(new Checkbox(config.ModoDios, ScreenCenter.X, 100));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.Niebla, ScreenCenter.X, 200));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.PostProcesadoCasco, ScreenCenter.X, 300));
            MenuOpciones.AgregarCheckbox(new Checkbox(config.MotionBlur, ScreenCenter.X, 400));

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
                //para que no se salga el mouse del Form
                var viewport = D3DDevice.Instance.Device.Viewport;
                var centroPantalla = new Point(DeviceWidth / 2, DeviceHeight / 2);
                System.Windows.Forms.Cursor.Position = centroPantalla;
            }
        }
    }
}
