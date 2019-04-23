using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShark.Variables
{
    public class Variable<T>
    {
        public string Nombre { get; }
        public T Valor { get; private set; }

        public Variable(string nombre, T valorInicial) {
            Nombre = nombre;
            Valor = valorInicial;
        }

        public void Actualizar(T valor) {
            Valor = valor;
        }
    }
}
