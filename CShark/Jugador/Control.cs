using Microsoft.DirectX.DirectInput;
using System;
using TGC.Core.Input;
using Action = System.Action;

namespace CShark.Jugador
{
    public class Control
    {
        private Key Tecla;
        private Action Accion;

        public Control(Key tecla, Action accion) {
            Tecla = tecla;
            Accion = accion;
        }

        public void Verificar(TgcD3dInput input) {
            if (input.keyDown(Tecla))
                Accion.Invoke();
        }
    }
}
