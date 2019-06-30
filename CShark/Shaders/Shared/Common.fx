#ifndef __COMMON_FX__
#define __COMMON_FX__
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float3x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float time = 0;
float alturaSuperficie = 18000;
float screen_dx;
float screen_dy;

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

struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

float calcularCoeficienteAcuatico(float altura)
{
    float distancia = alturaSuperficie - altura;
    return clamp(1 - distancia / alturaSuperficie, 0.7, 1);
}
#endif