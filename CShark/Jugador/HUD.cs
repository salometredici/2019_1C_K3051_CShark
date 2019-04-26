using CShark.UI.HUD;
using CShark.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;

namespace CShark.Jugador
{
    public class HUD
    {
        private BarraVida BarraVida;
        private BarraOxigeno BarraOxigeno;

        public HUD(float vida, float oxigeno) {
            int alturaTotal = D3DDevice.Instance.Device.Viewport.Height;
            BarraVida = new BarraVida(new TGCVector2(15, alturaTotal - 140), vida);
            BarraOxigeno = new BarraOxigeno(new TGCVector2(15, alturaTotal - 75), oxigeno);
        }

        public void Update(float vida, float oxigeno) {
            BarraVida.Update(vida);
            BarraOxigeno.Update(oxigeno);
        }

        public void Render() {
            BarraVida.Render();
            BarraOxigeno.Render();
        }
    }
}
