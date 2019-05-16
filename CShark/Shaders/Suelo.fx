//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

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

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;
    return Output;
}

float4 ps_main(float2 Texcoord : TEXCOORD0) : COLOR0
{
    return tex2D(diffuseMap, Texcoord);
}


technique SueloEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}