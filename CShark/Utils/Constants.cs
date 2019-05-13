using BulletSharp.Math;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;

namespace CShark.Utils
{
    public static class Constants
    {
        public static IList<Key> MovementKeys = new List<Key>() { Key.W, Key.S, Key.A, Key.D };
        public static Vector3 StandardGravity = new Vector3(0, -500f, 0);
        public static Vector3 UnderWaterGravity = new Vector3(0,-20f,0);
        public static IList<string> GuideLines = new List<string>()
        {
            "Guíar cámara - Mouse",
            "Impulsar hacia la izquierda - A",
            "Impulsar hacia la derecha - D",
            "Impulsar hacia adelante - W",
            "Impulsar hacia atrás - S",
            "Impulsar hacia arriba/Saltar (Fuera del agua) - Space",
            "Impulsar hacia abajo - X",
            "Frenar impulso - F",
            "Disparar - Click derecho",
            "Abrir/Cerrar menús - Esc",
            "Abrir menú de crafteo - E (Sólo en radio del bote)",
            "Abrir inventario - I"
        };
    }
}
