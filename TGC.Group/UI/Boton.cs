﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Text;
using System.Drawing;
using TGC.Group.Utils;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using System.Windows.Forms;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;
using TGC.Group.Model;

namespace TGC.Group.UI
{
    public class Boton
    {
        private CustomSprite Fondo;
        private Point Posicion;
        private TgcText2D Texto;
        private Drawer2D Drawer;
        private Action<GameModel> Accion;

        private CustomBitmap FondoNormal;
        private CustomBitmap FondoSeleccionado;
        private int Ancho;
        private int Alto;

        public Boton(string texto, int x, int y, Action<GameModel> accion) {
            Accion = accion;
            Drawer = new Drawer2D();
            CargarFondo();
            var centradoX = x - Ancho / 2;
            var centradoY = y + Alto / 2;
            Posicion = new Point(centradoX, centradoY);
            Fondo.Position = new TGCVector2(centradoX, centradoY);
            Texto = new TgcText2D
            {
                Align = TgcText2D.TextAlign.CENTER,
                Color = Color.White,
                Text = texto,
                Size = new Size(Ancho, Alto),
                Format = DrawTextFormat.VerticalCenter,
                Position = Posicion
            };
        }

        public void Update(GameModel juego) {
            Fondo.Bitmap = MouseAdentro() ? FondoSeleccionado : FondoNormal;
            if (Presionado(juego.Input))
                Accion.Invoke(juego);
        }

        private bool Presionado(TgcD3dInput input) {
            return input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT) && MouseAdentro();
        }

        public void Render() {
            Drawer.BeginDrawSprite();
            Drawer.DrawSprite(Fondo);
            Drawer.EndDrawSprite();
            Texto.render();
        }

        private void CargarFondo() {
            FondoNormal = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton1.png", D3DDevice.Instance.Device);
            FondoSeleccionado = new CustomBitmap(Game.Default.MediaDirectory + "\\Menu\\boton2.png", D3DDevice.Instance.Device);
            Ancho = 400;
            Alto = 75;
            Fondo = new CustomSprite();
            Fondo.Bitmap = FondoNormal;
            Fondo.Scaling = new TGCVector2((float)Ancho / FondoNormal.Width, (float)Alto / FondoNormal.Height);
        }

        private bool MouseAdentro() {
            return Cursor.Position.X > Posicion.X &&
            Cursor.Position.X < Posicion.X + Ancho &&
            Cursor.Position.Y > Posicion.Y &&
            Cursor.Position.Y < Posicion.Y + Alto;
        }
    }
}
