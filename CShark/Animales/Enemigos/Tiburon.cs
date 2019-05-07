using BulletSharp.Math;
using CShark.Animales;
using CShark.Animales.Comportamiento;
using CShark.Animales.Enemigos;
using CShark.Fisica;
using CShark.Geometria;
using CShark.Terreno;
using System;
using TGC.Core.Mathematica;

namespace CShark.NPCs.Enemigos
{
    public class Tiburon : Animal {

        private HealthBar BarraVida;
        private float TiempoInvencibilidad;        

        public Tiburon(TGCVector3 posicionInicial) : base("Tiburon", posicionInicial) {
            Comportamiento = new Aleatorio(Math.PI / 2, 300, 100, 300f, 1f);
            Vida = 300f;
            TiempoInvencibilidad = 0f;
            BarraVida = new HealthBar(Vida);
            var builder = new RigidBodyBuilder("Tiburon");
            Body = builder.ConDamping(1f).ConRotacion(Rotacion).ConPosicion(Posicion)
                .ConRebote(10f).ConRozamiento(1f).ConMasa(0f).Build();
            Mapa.Instancia.AgregarBody(Body);
        }

        public override void Update(float elapsedTime) {
            base.Update(elapsedTime);
            TiempoInvencibilidad -= elapsedTime;
            if (Vivo) BarraVida.Update(Vida, Posicion, Rotacion);
        }

        public override void Render() {
            base.Render();
            if (Vivo) BarraVida.Render(Vida);
        }

        public void RecibirDaño(float daño) {
            if (TiempoInvencibilidad <= 0) {
                Vida -= daño;
                TiempoInvencibilidad = 0f;
                if (Vida <= 0)
                    Morir();
            }
        }        

    }
}
