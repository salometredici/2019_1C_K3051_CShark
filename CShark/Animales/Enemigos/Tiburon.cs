using BulletSharp.Math;
using CShark.Animales;
using CShark.Animales.Comportamiento;
using CShark.Animales.Enemigos;
using CShark.Fisica;
using CShark.Geometria;
using CShark.Jugador;
using CShark.Model;
using CShark.Terreno;
using CShark.Utils;
using System;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Mathematica;
using TGC.Core.Sound;
using static CShark.Utils.EffectsPlayer;

namespace CShark.NPCs.Enemigos
{
    public class Tiburon : Animal {

        private HealthBar BarraVida;
        private float TiempoInvencibilidad;
        private bool Persiguiendo = false;
        private bool Mordio = false; //Para evitar que ataque cada ciclo
        private IComportamiento Aleatorio;

        public Tiburon(TGCVector3 posicionInicial) : base("Tiburon", posicionInicial) {
            Aleatorio = new Aleatorio(Math.PI / 2, 300, 100, 300f, 1f);
            Comportamiento = Aleatorio;
            Vida = 300f;
            TiempoInvencibilidad = 0f;
            BarraVida = new HealthBar(Vida);
            var builder = new RigidBodyBuilder("Tiburon");
            Body = builder.ConDamping(1f).ConRotacion(Rotacion).ConPosicion(posicionInicial)
                .ConRebote(10f).ConRozamiento(1f).ConMasa(0f).Build();
            Mapa.Instancia.AgregarBody(Body);
            Mesh.BoundingBox.setRenderColor(Color.Red);
            Mesh.AutoUpdateBoundingBox = true;
        }

        public override void Update(GameModel game) {
            base.Update(game);
            TiempoInvencibilidad -= game.ElapsedTime;
            var distanciaAlPlayer = TgcCollisionUtils.sqDistPointAABB(game.Player.Posicion, Mesh.BoundingBox);
            if (Vivo)
            {
                BarraVida.Update(Vida, Posicion, Rotacion);
            }
            if (PlayerDetectado(distanciaAlPlayer))
            {
                if (!Persiguiendo)
                {
                    Persiguiendo = !Persiguiendo;
                    MusicPlayer.SwitchMusic(false, Persiguiendo);
                    Comportamiento = new Perseguidor(0.1f, 1f, game.Player);
                }
                if (EnContacto(distanciaAlPlayer))
                {
                    if (!Mordio)
                    {
                        Atacar(game.Player);
                    }
                    Mesh.BoundingBox.setRenderColor(Color.Yellow);
                }
                else
                {
                    Mordio = !Mordio;
                    Mesh.BoundingBox.setRenderColor(Color.Green);
                }
            }
            else
            {
                if (Persiguiendo)
                {
                    Persiguiendo = false;
                    MusicPlayer.SwitchMusic(false, Persiguiendo);
                    Mordio = false;
                    Comportamiento = Aleatorio;
                }
                Mesh.BoundingBox.setRenderColor(Color.Red);
            }
        }

        public override void Render(GameModel game) {
            base.Render(game);
            if (Vivo) BarraVida.Render(Vida);
            Mesh.BoundingBox.Render();  //Sólo porque quería corroborar que funcionaba, se puede borrar
        }

        public void Atacar(Player player)
        {
            player.Atacado();
            EffectsPlayer.Play(SoundEffect.Bite);
            Mordio = true;
        }

        public void RecibirDaño(float daño) {
            if (TiempoInvencibilidad <= 0) {
                Vida -= daño;
                TiempoInvencibilidad = 0f;
                if (Vida <= 0)
                    Morir();
            }
        }
        public bool PlayerDetectado(float distanciaAlPlayer)
        {
            return FastMath.Sqrt(distanciaAlPlayer) < 7000f;            
        }

        public bool EnContacto(float distanciaAlPlayer)
        {
            return FastMath.Sqrt(distanciaAlPlayer) < 300f;
        }

    }
}
