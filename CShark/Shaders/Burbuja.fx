#include <Shared/Common.fx>
#include <Shared/Niebla.fx>

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float4 Color : TEXCOORD1;
    float Distancia : TEXCOORD2;
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
    Output.Distancia = 0;
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

VS_OUTPUT vs_burbi(VS_INPUT Input)
{
    VS_OUTPUT Output;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;
    Output.Distancia = mul(Input.Position, matWorldView).z;
    Output.Color = Input.Color;
    return (Output);
}


float4 ps_burbi(VS_OUTPUT input) : COLOR0
{
    //return calcularNiebla(input.Distancia, float4(0.49, 0.77, 1, 0.5));
    return float4(0.49, 0.77, 1, 0.5);

}

technique BurbujaAleatoria
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_burbi();
        PixelShader = compile ps_3_0 ps_burbi();
    }
}