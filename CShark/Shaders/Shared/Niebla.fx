#ifndef __NIEBLA_FX__
#define __NIEBLA_FX__
#include <Common.fx>
float4 colorNiebla;
float distanciaInicio;
float distanciaFin;
float densidad;

struct Niebla
{
    float3 Color;
    float DistanciaInicio;
    float DistanciaFin;
    float Densidad;
};

bool nublado(float distancia)
{
    return distancia > distanciaInicio && distanciaFin > distancia;
}

float4 calcularNiebla(float distancia, float altura, float4 color)
{
    if (nublado(distancia))
    {
        float total = distanciaFin - distanciaInicio;
        float resto = distancia - distanciaInicio;
        float niebla = 1 - resto / total;
        color = (1 - niebla) * colorNiebla + color * niebla;
    }

    float4 colorFinal = distancia > distanciaFin ? colorNiebla : color;

    if (altura < alturaSuperficie)
    {
        return colorFinal * calcularCoeficienteAcuatico(altura);
    }
    else
    {
        return colorFinal;
    }
}
#endif