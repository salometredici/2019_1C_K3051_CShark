using CShark.Luces.Materiales;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;

namespace CShark.Luces
{
    public static class Iluminacion
    {
        public static void AgregarLuz(Luz luz) {
            ContenedorLuces.Instancia.AgregarLuz(luz);
        }

        public static void SacarLuz(Luz luz) {
            ContenedorLuces.Instancia.SacarLuz(luz);
        }

        public static Effect EfectoLuz {
            get {
                return TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Luces.fx");
            }
        }

        public static void ActualizarEfecto(Effect efecto, IMaterial material, TGCVector3 camara) {
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
