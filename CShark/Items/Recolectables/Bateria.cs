using CShark.Luces;
using CShark.Luces.Materiales;
using CShark.Model;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Items.Recolectables
{
    public class Bateria : RecolectableAnimado
    {
        public override ERecolectable Tipo => ERecolectable.Bateria;
        private Effect Efecto;
        private IMaterial Material;
        private Luz Luz;

        public Bateria(TGCVector3 posicion) : base("Bateria", 4, posicion, 75f) {
            Efecto = Iluminacion.EfectoLuz;
            Efecto.Technique = "Iluminado";
            Material = new Metal();
            Luz = new Luz(Color.Red, posicion, 10f, 0.1f, 9f);
            Iluminacion.AgregarLuz(Luz);
        }

        public override void Render(GameModel game) {
            Iluminacion.ActualizarEfecto(Efecto, Material, game.Camara.Position);
            base.Render(game);
            if (!Recogido)
                Luz.Render();
        }

        public override void Update(GameModel game) {
            Luz.Update(Posicion);
            base.Update(game);
        }

        public override void Dispose() {
            Luz.Dispose();
            base.Dispose();
        }

        public override void Desaparecer() {
            Iluminacion.SacarLuz(Luz);
        }
    }
}
