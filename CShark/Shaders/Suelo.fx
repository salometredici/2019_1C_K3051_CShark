//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

texture texDiffuseMap;
texture texx;

sampler2D diffuseMap1 = sampler_state
{
    Texture = (texDiffuseMap);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

sampler2D diffuseMap2 = sampler_state
{
    Texture = (texx);
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

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float Rayitas : TEXCOORD1;
};


VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;
    float alturaSuperficie = 18000.0f;
    float tamanioTile = 64.0f;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord * tamanioTile;
    Output.Rayitas = Input.Position.y < alturaSuperficie ? 0.8 : 0.3; //si estoy bajo agua veo MÁS los reflejitos
    return Output;
}

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
    float plano = 1 - Input.Rayitas;
    float3 color1 = tex2D(diffuseMap1, Input.Texcoord).rgb;
    float3 color2 = tex2D(diffuseMap2, Input.Texcoord).rgb;
    return float4(color1 * plano + color2 * Input.Rayitas, 1);
}

technique SueloEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}