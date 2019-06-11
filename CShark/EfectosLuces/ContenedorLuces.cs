﻿using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.EfectosLuces
{
    public class ContenedorLuces
    {
        private List<Luz> Luces;
        private IEnumerable<Luz> LucesPosta;
        public int Cantidad { get; private set; }
        public ColorValue[] Colores { get; private set; }
        public Vector4[] Posiciones { get; private set; }
        public float[] Intensidades { get; private set; }
        public float[] Atenuaciones { get; private set; }
        public float[] Especulares { get; private set; }

        private const int N = 10;

        public static ContenedorLuces Instancia { get; } = new ContenedorLuces();

        private ContenedorLuces() {
            Luces = new List<Luz>();
            LucesPosta = new List<Luz>();
            Colores = new ColorValue[N];
            Intensidades = new float[N];
            Posiciones = new Vector4[N];
            Atenuaciones = new float[N];
            Especulares = new float[N];
        }

        public void AgregarLuz(Luz luz) {
            Luces.Add(luz);
        }

        public void SacarLuz(Luz luz) {
            Luces.Remove(luz);
        }

        float DistanciaCercania = 20000f;

        public void Update(TGCVector3 player) {
            LucesPosta = Luces
                .Where(luz => luz.Distancia(player) < DistanciaCercania)
                .Take(N);
            ArmarLuces();
        }

        public void ArmarLuces() {
            Cantidad = Math.Min(LucesPosta.Count(), N);
            for (int i = 0; i < Cantidad; i++) {
                Colores[i] = ColorValue.FromColor(LucesPosta.ElementAt(i).Color);
                Intensidades[i] = LucesPosta.ElementAt(i).Intensidad;
                Atenuaciones[i] = LucesPosta.ElementAt(i).Atenuacion;
                Posiciones[i] = TGCVector3.Vector3ToVector4(LucesPosta.ElementAt(i).Posicion);
            }
        }
    }
}
