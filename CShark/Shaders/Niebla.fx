#include <Shared/Common.fx>
#include <Shared/Niebla.fx>
#include <Shared/Iluminacion.fx>
#include <Shared/Suelo.fx>
#include <Shared/Olas.fx>

struct VS_INPUT_NIEBLA
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_NIEBLA
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
    float Distancia : TEXCOORD3;
};

VS_OUTPUT_NIEBLA vertex_nublado(VS_INPUT_NIEBLA input)
{
    VS_OUTPUT_NIEBLA output;
    output.Position = mul(input.Position, matWorldViewProj);
    output.Distancia = mul(input.Position, matWorldView).z;
    output.Texcoord = input.Texcoord;
    output.WorldPosition = mul(input.Position, matWorld);
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;
    return output;
}

float4 pixel_nublado(VS_OUTPUT_NIEBLA input) : COLOR0
{
    float4 texel = tex2D(diffuseMap, input.Texcoord);
    return calcularNiebla(input.Distancia, texel);
}

technique Nublado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_nublado();
        PixelShader = compile ps_3_0 pixel_nublado();
    }
}

VS_OUTPUT_NIEBLA vertex_iluminado_nublado(VS_INPUT_NIEBLA input)
{
    VS_OUTPUT_NIEBLA output;
    output.Position = mul(input.Position, matWorldViewProj);
    output.Distancia = mul(input.Position, matWorldView).z;
    output.Texcoord = input.Texcoord;
    output.WorldPosition = mul(input.Position, matWorld);
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;
    return output;
}

float4 pixel_iluminado_nublado(VS_OUTPUT_NIEBLA input) : COLOR0
{
    float4 texel = tex2D(diffuseMap, input.Texcoord);
    float4 color = calcularLuces(input.WorldNormal, input.WorldPosition, texel.rgb);
    return calcularNiebla(input.Distancia, color);
}

float4 ps_suelo_nublado(VS_OUTPUT_SUELO Input) : COLOR0
{
    float4 texel = calcularBlendRayos(Input.Rayitas, Input.Texcoord);
    float4 luces = calcularLuces(Input.WorldNormal, Input.WorldPosition, texel.rgb);
    return calcularNiebla(Input.Distancia, luces);
}

technique SueloNublado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_suelo();
        PixelShader = compile ps_3_0 ps_suelo_nublado();
    }
}

float4 ps_olas_nublado(VS_OUTPUT_OLAS Input) : COLOR0
{
    float4 tex = tex2D(diffuseMap, Input.Texcoord);
    float distancia = Input.Distancia;
    float3 color = calcularNiebla(distancia, tex);
    return float4(color, Input.Alpha);
}

technique OlasNublado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_olas();
        PixelShader = compile ps_3_0 ps_olas_nublado();
    }
}