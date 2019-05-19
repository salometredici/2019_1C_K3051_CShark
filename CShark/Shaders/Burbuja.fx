#include <Shared/Common.fx>

float time = 0;

struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float4 Color : TEXCOORD1;
};


VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;
    float red = Input.Color.r * cos(time) * 1.3 * Input.Position.x;
    float blue = Input.Color.b * cos(time) * 1.9 * Input.Position.y;
    float green = Input.Color.g * cos(time) * 0.5 * Input.Position.z;
    float alpha = clamp(Input.Color.a * sin(time), 0.3, 0.8);
    Output.Color = float4(red, blue, green, alpha);
    return (Output);
}

float4 ps_main(float4 Color : TEXCOORD1) : COLOR0
{
    return Color;
}

technique Burbuja
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}