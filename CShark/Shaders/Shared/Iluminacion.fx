#ifndef __ILUMINACION_FX__
#define __ILUMINACION_FX__
#include <Common.fx>
float3 coloresLuces[10];
float3 posicionesLuces[10];
float intensidadesLuces[10];
float atenuacionesLuces[10];

//Cantidad real de luces 
int cantidadLuces;

//Propiedades del material
float4 colorDifuso;
float3 colorEmisivo;
float3 colorAmbiente;
float3 colorEspecular;
float exponenteEspecular;

//Posicion de la camara
float4 posicionCamara;

struct Luz
{
    float3 Color;
    float3 Posicion;
    float Intensidad;
    float Atenuacion;
};

Luz getLuz(int i)
{
    Luz luz;
    luz.Color = coloresLuces[i];
    luz.Atenuacion = atenuacionesLuces[i];
    luz.Intensidad = intensidadesLuces[i];
    luz.Posicion = posicionesLuces[i];
    return luz;
};

float4 calcularLuces(float3 worldNormal, float3 worldPosition, float3 texel)
{
    float3 Nn = normalize(worldNormal);
    float3 luzEspecular = 0;
    float3 luzAmbiente = 0;
    float3 luzDifusa = colorEmisivo;

    for (int i = 0; i < cantidadLuces; i++)
    {
        Luz luz = getLuz(i);

        float3 Ln = normalize(luz.Posicion - worldPosition);
        float3 Hn = normalize(posicionCamara.xyz - 2 * worldPosition + luz.Posicion);
        float3 n_dot_l = max(0.0, dot(Nn, Ln));
        float3 n_dot_h = max(0.0, dot(Nn, Hn));

        float distanciaAtenuada = length(luz.Posicion - worldPosition) * luz.Atenuacion;
        float intensidad = luz.Intensidad / distanciaAtenuada;
        
        luzAmbiente += intensidad * luz.Color;
        luzDifusa += intensidad * luz.Color * n_dot_l;
        luzEspecular += intensidad * luz.Color * pow(n_dot_h, exponenteEspecular);
    }

    luzEspecular *= colorEspecular;
    luzAmbiente *= colorAmbiente;
    luzDifusa *= colorDifuso;
       
    float4 colorFinal = float4(saturate(colorEmisivo + luzDifusa + luzAmbiente) * texel + luzEspecular, colorDifuso.a);

    if (worldPosition.y < alturaSuperficie)
    {
        return colorFinal * calcularCoeficienteAcuatico(worldPosition.y);
    }
    else
    {
        return colorFinal;
    }
}
#endif