using CShark.Items;
using CShark.Items.Crafteables;
using CShark.Jugador;
using CShark.Model;
using CShark.UI.HUD;
using CShark.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Text;

namespace CShark.UI
{
    public class MenuCrafteo : ItemsMenus, IDisposable
    {
        private BotonCraftear Boton;
        private List<BotonItem> Botones;
        public List<TgcText2D> Requerimientos;
        private ICrafteable Seleccionado;

        public MenuCrafteo(string rutaFondo) {
            base.Init(rutaFondo, "");
            float anchoReal = Fondo.Bitmap.ImageInformation.Width;
            float altoReal = Fondo.Bitmap.ImageInformation.Height;
            Boton = new BotonCraftear(Fondo.Position, anchoReal, altoReal);
            CargarBotones(Fondo.Position);
            Requerimientos = new List<TgcText2D>();
        }

        private void CargarBotones(TGCVector2 posInicial) {
            var pos = posInicial + new TGCVector2(29, 29);
            var desplazamColumna = new TGCVector2(179, 0);
            var desplazamFila = new TGCVector2(-358, 179);
            Botones = new List<BotonItem>();
            Botones.Add(new BotonItem(new Arpon(), pos, this));
            pos += desplazamColumna;
            Botones.Add(new BotonItem(new Oxigeno(), pos, this));
            pos += desplazamColumna;
            Botones.Add(new BotonItem(new Medkit(), pos, this));
            pos += desplazamFila;
            Botones.Add(new BotonItem(new Oro(), pos, this));
            pos += desplazamColumna;
            Botones.Add(new BotonItem(new AumentoOxigeno(), pos, this));
            pos += desplazamColumna;
            Botones.Add(new BotonItem(new AumentoVida(), pos, this));
        }

        public void CambiarItem(BotonItem boton) {
            Botones.ForEach(b => b.Seleccionado = false);
            boton.Seleccionado = true;
            Seleccionado = boton.Item;
            Requerimientos = ArmarRequerimientos(player);
        }

        public void Craftear() {
            if (Seleccionado.PuedeCraftear(player)) {
                player.CraftearItem(Seleccionado);
                Requerimientos = ArmarRequerimientos(player);
            }
        }

        public override void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Cerrar.Render(Drawer);
            Boton.Render(Drawer);
            Botones.ForEach(b => b.Render(Drawer));
            Drawer.EndDrawSprite();
            Titulo.render();
            Requerimientos.ForEach(r => r.render());
        }

        public override void Update(GameModel juego) {
            base.Update(juego);
            Boton.Update(this, juego.Input);
            Botones.ForEach(b => b.Update(juego, this));
            if (Seleccionado != null)
                Titulo.Text = TituloDisplay();
        }
        private string TituloDisplay()
        {
            switch (Seleccionado.Tipo)
            {
                case (ECrafteable.Arpon):
                    return "Arpón";
                case (ECrafteable.AumentoOxigeno):
                    return "Aumento de Oxígeno";
                case (ECrafteable.AumentoVida):
                    return "Aumento de Vida";
                default:
                    return Seleccionado.Tipo.ToString();
            }
        }

        private List<TgcText2D> ArmarRequerimientos(Player player) {
            var texto = string.Empty;
            var lineas = new List<TgcText2D>();
            var pos = new Point(Titulo.Position.X, Titulo.Position.Y + 39);
            foreach (var material in Seleccionado.Materiales) {
                var tipo = material.Key;
                var disponibles = player.CuantosTiene(tipo);
                var necesarios = material.Value;
                var linea = new TgcText2D {
                    Align = TgcText2D.TextAlign.LEFT,
                    Position = pos,
                    Size = new Size(Ancho, 20)
                };
                linea.changeFont(new Font("Arial", 16f, FontStyle.Regular));
                linea.Color = disponibles >= necesarios ? Color.LightGreen : Color.Red;
                linea.Text = tipo.ToString() + string.Format(" ({0}/{1})", disponibles, necesarios);
                lineas.Add(linea);
                pos = new Point(pos.X, pos.Y + 30);
            }
            return lineas;
        }

        public new void Dispose() {
            base.Dispose();
            Boton.Dispose();
            Requerimientos.ForEach(r => r.Dispose());            
            Botones.ForEach(b => b.Dispose());
        }
    }
}
