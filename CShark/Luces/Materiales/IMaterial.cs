using Microsoft.DirectX.Direct3D;

namespace CShark.Luces.Materiales
{
    public interface IMaterial
    {
        ColorValue Difuso { get; }
        ColorValue Emisivo { get; }
        ColorValue Ambiente { get; }
        ColorValue Especular { get; }
        float Brillito { get; } //specularExp
    }
}
