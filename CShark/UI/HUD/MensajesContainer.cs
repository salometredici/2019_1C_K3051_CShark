using CShark.Items;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.UI.HUD
{
    public class MensajesContainer : IDisposable
    {
        private List<MensajePlus> Mensajes;
        private MensajePlus Activo;

        public MensajesContainer() {
            Initialize();
        }

        private void Initialize() {
            Mensajes = new List<MensajePlus>();
            Mensajes.Add(new MensajePlus("burbuja.png", "Burbuja", this));
            Mensajes.Add(new MensajePlus("chip.png", "Chip", this));
            Mensajes.Add(new MensajePlus("pila.png", "Pila", this));
            Mensajes.Add(new MensajePlus("wumpa.png", "Wumpa", this));
        }

        public void Update(float elapsedTime) {
            if (Activo != null)
                Activo.Update(elapsedTime);
        }

        public void Render(Drawer2D drawer) {
            if (Activo != null)
                Activo.Render(drawer);
        }

        public void Desactivar() {
            Activo = null;
        }

        public void PopMensaje(ERecolectable tipo) {
            var mensaje = GetMensaje(tipo);
            Activo = mensaje;
            Activo.Activar();
        }

        private MensajePlus GetMensaje(ERecolectable tipo) {
            switch (tipo) {
                case ERecolectable.Burbuja: return Mensajes[0];
                case ERecolectable.Chip: return Mensajes[1];
                case ERecolectable.Pila: return Mensajes[2];
                case ERecolectable.Wumpa: return Mensajes[3];
                default: return null;
            }
        }

        public void Dispose() {
            Mensajes.ForEach(m => m.Dispose());
        }
    }
}
