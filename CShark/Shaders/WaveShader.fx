//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float time = 0;

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
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float2 RealPos : TEXCOORD1;
    float4 Color : COLOR0;
};


VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;

    Output.RealPos = Input.Position;

    float altura = 2800;
   
    Input.Position.x += sin(time) * 30;
    Input.Position.y += cos(time) * sign(Input.Position.y - 20);
    float variacion = Input.Position.y > 0 ? cos(time) * 1.1 : cos(time) * 0.9;
    Input.Position.y *= variacion;
    Input.Position.y += altura;

    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;
    Output.Color = Input.Color;
    return (Output);
}

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
    return tex2D(diffuseMap, Input.Texcoord);
}

technique WaveEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}