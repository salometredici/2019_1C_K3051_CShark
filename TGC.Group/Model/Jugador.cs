using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Group.UI.HUD;
using TGC.Group.Utils;

namespace TGC.Group.Model
{
    public class Jugador
    {
        public int Vida;
        public int Oxigeno;
        private List<IRecolectable> Recolectables;
        public TGCVector3 Posicion { get; private set; }

        private Drawer2D Drawer;
        private BarraVida BarraVida;
        private BarraOxigeno BarraOxigeno;
        
        public Jugador(TGCVector3 posicion, int vidaInicial, int oxigenoInicial) {
            Recolectables = new List<IRecolectable>();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            Drawer = new Drawer2D();
            CargarHUD();
        }

        private void CargarHUD() {
            int alturaTotal = D3DDevice.Instance.Device.Viewport.Height;
            BarraVida = new BarraVida(new TGCVector2(15, alturaTotal - 140), Vida);
            BarraOxigeno = new BarraOxigeno(new TGCVector2(15, alturaTotal - 75), Oxigeno);
        }
        
        public void Update(TgcCamera camara) {
            Posicion = camara.Position;
            //Vida -= 1;
            Oxigeno -= 7;
            BarraVida.Update(Vida);
            BarraOxigeno.Update(Oxigeno);
        }

        public void Render() {
            BarraVida.Render();
            BarraOxigeno.Render();
        }

    }
}
