#ifndef __COMMON_FX__
#define __COMMON_FX__
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float3x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float time = 0;
float alturaSuperficie = 18000;

texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
    Texture = (texDiffuseMap);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

float calcularCoeficienteAcuatico(float altura)
{
    float distancia = alturaSuperficie - altura;
    return clamp(1 - distancia / alturaSuperficie, 0.7, 1);
}
#endif