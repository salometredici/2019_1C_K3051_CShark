using CShark.Geometria;
using CShark.Jugador;
using System;
using TGC.Core.Mathematica;

namespace CShark.Animales.Comportamiento
{
    public class Perseguidor : IComportamiento
    {
        public float VelocidadRotacion;
        public float VelocidadMovimiento;

        private Player Objetivo;

        public Perseguidor(float vMovimiento, float vRotacion, Player player)
        {
            VelocidadMovimiento = vMovimiento;
            VelocidadRotacion = vRotacion;
            Objetivo = player;
        }

        public void Update(float elapsedTime, Animal animal)
        {
            AvanzarHaciaPlayer(elapsedTime, animal);
        }

        public void Render()
        {

        }

        private void AvanzarHaciaPlayer(float elapsedTime, Animal animal)
        {
            var direccionDespl = Objetivo.Posicion - animal.Posicion;
            var desplazam = direccionDespl * VelocidadMovimiento * 0.1f;
            if (direccionDespl.Length() > 100f && animal.Posicion.Y < 18000f)
            {
                animal.Posicion += desplazam;
            }

        }

    }
}

