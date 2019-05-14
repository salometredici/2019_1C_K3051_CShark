using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.Mathematica;

namespace CShark.Jugador
{
    public class InputHandler
    {
        private Player Player;
        private List<Control> Controles;

        public InputHandler(Player player) {
            Player = player;
            CargarAcciones();
        }

        private void CargarAcciones() {
            Controles = new List<Control>();
            Controles.Add(new Control(Key.W, () => Impulsar(-1, 0)));
            Controles.Add(new Control(Key.A, () => Impulsar(1, FastMath.PI_HALF)));
            Controles.Add(new Control(Key.S, () => Impulsar(1, 0)));
            Controles.Add(new Control(Key.D, () => Impulsar(-1, FastMath.PI_HALF)));
            Controles.Add(new Control(Key.Space, () => Vertical(1)));
            Controles.Add(new Control(Key.LeftControl, () => Vertical(-1)));
        }

        public void Update() {
            Controles.ForEach(c => c.Verificar(Player.Input));
        }

        private void Vertical(int sentido) {
            if (Player.Sumergido)
                Player.Flotar(sentido);
            else Player.Saltar();
        }

        private void Impulsar(int sentido, float offsetAngular) {
            float angulo = Player.CamaraInterna.leftrightRot + offsetAngular;
            float x = FastMath.Sin(angulo) * sentido;
            float z = FastMath.Cos(angulo) * sentido;
            Player.MoverCapsula(x, 0, z);
        }
    }
}
