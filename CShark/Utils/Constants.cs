using BulletSharp.Math;
using CShark.Items;
using CShark.Items.Recolectables;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using TGC.Core.Mathematica;

namespace CShark.Utils
{
    public static class Constants
    {
        public static IList<Key> MovementKeys = new List<Key>() { Key.W, Key.S, Key.A, Key.D };
        public static Vector3 StandardGravity = new Vector3(0, -5000f, 0);
        public static Vector3 UnderWaterGravity = new Vector3(0,-2000f,0);
        public static IList<string> GuideLines = new List<string>()
        {
            "Guíar cámara - Mouse",
            "Impulsar hacia la izquierda - A",
            "Impulsar hacia la derecha - D",
            "Impulsar hacia adelante - W",
            "Impulsar hacia atrás - S",
            "Impulsar hacia arriba/Saltar - Space",
            "Disparar - Click derecho",
            "Abrir/Cerrar menús - Esc",
            "Abrir menú de crafteo - E (Sólo en mesa de crafteo)",
            "Recoger item - E",
            "Abrir inventario - I"
        };
        public static IList<Recolectable> CheatItems = new List<Recolectable>()
        {
            new Oro(TGCVector3.Empty),
            new Oxigeno(TGCVector3.Empty),
            new Wumpa(TGCVector3.Empty),
            new Plata(TGCVector3.Empty),
            new Pez(TGCVector3.Empty),
            new Pila(TGCVector3.Empty),
            new Medkit(TGCVector3.Empty),
            new Hierro(TGCVector3.Empty),
            new Coral(TGCVector3.Empty),
            new Burbuja(TGCVector3.Empty),
            new Chip(TGCVector3.Empty),
            new Arpon(TGCVector3.Empty)
        };
    }
}
