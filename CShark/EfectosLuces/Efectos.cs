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
        public static void AgregarLuz(Luz luz) {
            ContenedorLuces.Instancia.AgregarLuz(luz);
        }

        public static void SacarLuz(Luz luz) {
            ContenedorLuces.Instancia.SacarLuz(luz);
        }

        public static Effect EfectoLuz {
            get {
                return TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Iluminacion.fx");
            }
        }

        public static Effect EfectoCasco
        {
            get
            {
                return TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Casco.fx");
            }
        }

        public static Effect EfectoLuzNiebla { get; } = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Niebla.fx");

        //efecto parametro porque puede ser la niebla del suelo q es otro efecto..
        public static void ActualizarNiebla() {
            var distanciaInicio = 10000;
            var distanciaFin = 20000;
            var densidad = 0.0025f;
            var colorNiebla = Color.Black;

            EfectoLuzNiebla.SetValue("colorNiebla", colorNiebla.ToArgb());
            EfectoLuzNiebla.SetValue("distanciaInicio", distanciaInicio);
            EfectoLuzNiebla.SetValue("distanciaFin", distanciaFin);
            EfectoLuzNiebla.SetValue("densidad", densidad);
        }

        public static void ActualizarLuces(Effect efecto, Material material, TGCVector3 camara) {
            var contenedor = ContenedorLuces.Instancia;

            //cargar propiedades de las luces
            efecto.SetValue("coloresLuces", contenedor.Colores);
            efecto.SetValue("posicionesLuces", contenedor.Posiciones);
            efecto.SetValue("intensidadesLuces", contenedor.Intensidades);
            efecto.SetValue("atenuacionesLuces", contenedor.Atenuaciones);
            efecto.SetValue("cantidadLuces", contenedor.Cantidad);

            //cargar propiedades del material
            efecto.SetValue("colorEmisivo", material.Emisivo);
            efecto.SetValue("colorDifuso", material.Difuso);
            efecto.SetValue("colorEspecular", material.Especular);
            efecto.SetValue("exponenteEspecular", material.Brillito);
            efecto.SetValue("colorAmbiente", material.Ambiente);

            //cargar posicion de la camara
            efecto.SetValue("posicionCamara", TGCVector3.Vector3ToFloat4Array(camara));
        }
    }
}
