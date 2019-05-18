using Microsoft.DirectX.Direct3D;

namespace CShark.Objetos
{
    public class Material
    {
        public ColorValue Difuso { get; set; }
        public ColorValue Emisivo { get; set; }
        public ColorValue Ambiente { get; set; }
        public ColorValue Especular { get; set; }
        public float Brillito { get; set; } //specularExp
    }
}
