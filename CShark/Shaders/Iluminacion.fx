#include <Shared/Common.fx>
#include <Shared/Iluminacion.fx>
#include <Shared/Niebla.fx>
#include <Shared/Suelo.fx>
#include <Shared/Olas.fx>

struct VS_INPUT_ILUMINADO
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_ILUMINADO
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};


VS_OUTPUT_ILUMINADO vertex_iluminado(VS_INPUT_ILUMINADO input)
{
    VS_OUTPUT_ILUMINADO output;
    output.Position = mul(input.Position, matWorldViewProj);
    output.Texcoord = input.Texcoord;
    output.WorldPosition = mul(input.Position, matWorld);
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;
    return output;
}

float4 pixel_iluminado(VS_OUTPUT_ILUMINADO input) : COLOR0
{
    float3 colorTexel = tex2D(diffuseMap, input.Texcoord);
    return calcularLuces(input.WorldNormal, input.WorldPosition, colorTexel);
}

technique Iluminado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_iluminado();
        PixelShader = compile ps_3_0 pixel_iluminado();
    }
}

float4 ps_suelo_iluminado(VS_OUTPUT_SUELO Input) : COLOR0
{
    float4 texel = calcularBlendRayos(Input.Rayitas, Input.Texcoord);
    return calcularLuces(Input.WorldNormal, Input.WorldPosition, texel.rgb);
}

technique SueloIluminado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_suelo();
        PixelShader = compile ps_3_0 ps_suelo_iluminado();
    }
}

float4 ps_olas(VS_OUTPUT_OLAS Input) : COLOR0
{
    float3 color = tex2D(diffuseMap, Input.Texcoord).rgb;
    return float4(color, Input.Alpha);
}


//hacer iluminacion FALTAA
technique OlasIluminado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_olas();
        PixelShader = compile ps_3_0 ps_olas();
    }
}