using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using CShark.UI.HUD;
using CShark.Utils;
using CShark.Model;

namespace CShark.Jugador
{
    public class Player
    {
        public float Vida;
        public float Oxigeno;
        private Inventario Inventario;
        private HUD HUD;
        public TGCVector3 Posicion { get; private set; }
        public bool EstaVivo => Vida > 0 && Oxigeno > 0;
        
        public Player(TGCVector3 posicion, int vidaInicial, int oxigenoInicial) {
            Inventario = new Inventario();
            Posicion = posicion;
            Vida = vidaInicial;
            Oxigeno = oxigenoInicial;
            HUD = new HUD(Vida, Oxigeno);
        }
        
        public void Update(TgcFpsCamera camara, float elapsedTime) {
            Posicion = camara.Position;
            Oxigeno -= 200f * elapsedTime;
            if (EstaVivo)
            {
                HUD.Update(Vida, Oxigeno);
                VerificarColisiones();
            }
            else
                BloquearCamara(camara);
        }

        private void VerificarColisiones() {

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
                HUD.Render();
        }

    }
}
