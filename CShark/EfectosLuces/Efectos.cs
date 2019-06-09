using CShark.Objetos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using Material = CShark.Objetos.Material;

namespace CShark.EfectosLuces
{
    public class Efectos
    {
        public static Efectos Instancia { get; } = new Efectos();

        public float distanciaNiebla { get; set; } = 40000;
        public Color colorNiebla { get; set; } = Color.LightGray;

        public Effect EfectoLuz { get; } = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Iluminacion.fx");

        public Effect EfectoLuzNiebla { get; } = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Niebla.fx");

        public void AgregarLuz(Luz luz) {
            ContenedorLuces.Instancia.AgregarLuz(luz);
        }

        public void SacarLuz(Luz luz) {
            ContenedorLuces.Instancia.SacarLuz(luz);
        }

        public void ActualizarNiebla() {
            var distanciaInicio = 10000;
            var distanciaFin = distanciaNiebla;
            var densidad = 0.0025f;

            EfectoLuzNiebla.SetValue("colorNiebla", colorNiebla.ToArgb());
            EfectoLuzNiebla.SetValue("distanciaInicio", distanciaInicio);
            EfectoLuzNiebla.SetValue("distanciaFin", distanciaFin);
            EfectoLuzNiebla.SetValue("densidad", densidad);
        }

        public void ActualizarLuces(Effect efecto, Material material, TGCVector3 camara) {
            
        }
    }
}
