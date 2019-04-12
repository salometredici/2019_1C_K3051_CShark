using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Variables
{
    public class Variable
    {
        public string Nombre { get; }
        public float Valor { get; private set; }

        public Variable(string nombre, float valorInicial) {
            Nombre = nombre;
            Valor = valorInicial;
        }

        public void Actualizar(float valor) {
            Valor = valor;
        }
    }
}
