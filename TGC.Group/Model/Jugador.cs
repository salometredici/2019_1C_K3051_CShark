﻿using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
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
        public bool EstaVivo => Vida > 0 && Oxigeno > 0;

        private Drawer2D Drawer;
        private BarraVida BarraVida;
        private BarraOxigeno BarraOxigeno;
        private TgcMesh Brazo;
        
        public Jugador(TGCVector3 posicion, int vidaInicial, int oxigenoInicial) {
            Recolectables = new List<IRecolectable>();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            Drawer = new Drawer2D();
            CargarHUD();
            CargarMeshes();
        }

        private void CargarHUD() {
            int alturaTotal = D3DDevice.Instance.Device.Viewport.Height;
            BarraVida = new BarraVida(new TGCVector2(15, alturaTotal - 140), Vida);
            BarraOxigeno = new BarraOxigeno(new TGCVector2(15, alturaTotal - 75), Oxigeno);
        }

        private void CargarMeshes() {
            var loader = new TgcSceneLoader();
            Brazo = loader.loadSceneFromFile(Game.Default.MediaDirectory + "intentoDeBrazo-TgcScene.xml").Meshes[0];
            Brazo.Position = Posicion;
            float tamaño = 0.001f;
            Brazo.Scale = new TGCVector3(tamaño, tamaño, tamaño);
            Brazo.RotateX((float)Math.PI / 2);
        }
        
        public void Update(TgcFpsCamera camara) {
            Posicion = camara.Position;
            PosicionarBrazo();
            Oxigeno -= 2;
            if (EstaVivo)
            {
                BarraVida.Update(Vida);
                BarraOxigeno.Update(Oxigeno);
            }
            else
            {
                BloquearCamara(camara);
            }
        }

        private void PosicionarBrazo() {

        }

        private bool _murio = false;
        private void BloquearCamara(TgcFpsCamera camara) {
            if (!_murio)
            {
                camara.Lock();
                _murio = true;
            }
        }

        public void Render() {
            if (EstaVivo)
            {
                BarraVida.Render();
                BarraOxigeno.Render();
                Brazo.Render();
            }
        }

    }
}
