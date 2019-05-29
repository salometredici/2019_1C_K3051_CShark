using CShark.Items;
using CShark.Items.Crafteables;
using CShark.Items.Recolectables;
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
    public class MenuInventario : ItemsMenus, IDisposable
    {
        private List<BotonInventario> Botones;
        private TgcText2D InfoItem;
        private ERecolectable Seleccionado ;

        public MenuInventario(string rutaFondo)
        {
            base.Init(rutaFondo, "");
            CargarBotones(Fondo.Position);
            Titulo.Position = new Point((int)Fondo.Position.X + 537 + 29, (int)Fondo.Position.Y + 29);
            InfoItem = new TgcText2D();
        }

        private void CargarBotones(TGCVector2 posInicial)
        {
            var pos = posInicial + new TGCVector2(29, 29);
            var desplazamColumna = new TGCVector2(179, 0);
            var desplazamFila = new TGCVector2(-358, 179);
            Botones = new List<BotonInventario>
            {
                new BotonInventario(ERecolectable.Arpon, pos)
            };
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Oxigeno, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Medkit, pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario(ERecolectable.Oro, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Plata, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Hierro, pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario(ERecolectable.Wumpa, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Coral, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Pez, pos));
            pos += desplazamFila;
            Botones.Add(new BotonInventario(ERecolectable.Chip, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Pila, pos));
            pos += desplazamColumna;
            Botones.Add(new BotonInventario(ERecolectable.Burbuja,pos));

        }

        public void CambiarItem(BotonInventario boton)
        {
            Botones.ForEach(b => b.Seleccionado = false);
            boton.Seleccionado = true;
            Seleccionado = boton.Item;
            InfoItem = CantDisponibleDisplay(player);
        }
        private TgcText2D CantDisponibleDisplay(Player player)
        {
            var pos = new Point(Titulo.Position.X - 329, Titulo.Position.Y + 39);
            var cantDisponible = player.CuantosTiene(Seleccionado);
            var linea = new TgcText2D()
            {
                Align = TgcText2D.TextAlign.CENTER,
                Position = pos,
                Text = "Cantidad: " + cantDisponible.ToString(),
                Size = new Size(Ancho, 20),
                Color = cantDisponible > 0 ? Color.Yellow : Color.Red
            };
            linea.changeFont(new Font("Arial", 16f, FontStyle.Regular));
            return linea;
        }

        public override void Render()
        {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Cerrar.Render(Drawer);
            Botones.ForEach(b => b.Render(Drawer));
            Drawer.EndDrawSprite();
            Titulo.render();
            InfoItem.render();
        }

        public override void Update(GameModel juego)
        {
            base.Update(juego);
            Botones.ForEach(b => b.Update(juego, this));
            Titulo.Text = Seleccionado.ToString();
        }

        public new void Dispose()
        {
            base.Dispose();
            InfoItem.Dispose();
            Botones.ForEach(b => b.Dispose());
        }
    }
}
