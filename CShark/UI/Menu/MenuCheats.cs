using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using CShark.Model;
using static CShark.Model.GameModel;
using CShark.Utils;

namespace CShark.UI
{
    public class MenuCheats : Menu
    {
        private List<Boton> Botones;
        private int Separacion; //entre botones
        private int AlturaBotones;

        public int CantidadBotones => Botones.Count();

        public MenuCheats() : base()
        {
            Botones = new List<Boton>();
            Separacion = 50;
            AlturaBotones = 75;
        }

        public void AgregarBoton(string texto, Action<GameModel> accion)
        {
            var posicionX = DeviceWidth / 2 + DeviceWidth / 4;
            var posicionY = CantidadBotones > 0 ? Botones.Last().Posicion.Y + Separacion : (int)RightMenuPos_Y + Separacion;
            Botones.Add(new Boton(texto, posicionX, posicionY, accion));
        }

        public override void Update(GameModel juego)
        {
            Botones.ForEach(b => b.Update(juego));
        }

        public override void Render()
        {
            base.Render();
            Botones.ForEach(b => b.Render());
        }

    }
}
