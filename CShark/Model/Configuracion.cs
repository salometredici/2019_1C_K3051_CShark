using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShark.Model;
using CShark.Variables;

namespace CShark.Model
{
    public class Configuracion
    {
        public Variable<float> VelocidadMovimiento;
        public Variable<float> VelocidadRotacion;
        public Variable<bool> ModoDios;
        public Variable<bool> Niebla;
        public Variable<bool> PostProcesadoCasco;
        public Variable<bool> MotionBlur;
        public Variable<float> FPS;
        public Variable<bool> MostrarRayo;

        private static Configuracion instancia;

        public static Configuracion Instancia {
            get {
                if (instancia == null)
                    instancia = new Configuracion();
                return instancia;
            }
        }

        private Configuracion() {
            VelocidadMovimiento = new Variable<float>("Velocidad de movimiento", 500f);
            VelocidadRotacion = new Variable<float>("Velocidad de rotación", 0.1f);
            ModoDios = new Variable<bool>("Modo Dios", true);
            Niebla = new Variable<bool>("Niebla", false);
            PostProcesadoCasco = new Variable<bool>("Post Procesado (Casco)", false);
            MotionBlur = new Variable<bool>("Motion Blur", false);
            FPS = new Variable<float>("Cuadros por segundo", 60f);
            MostrarRayo = new Variable<bool>("Mostrar rayo de agarre", false);
        }
    }
}
