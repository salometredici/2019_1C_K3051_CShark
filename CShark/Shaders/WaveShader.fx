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
    
    float altura = 2800;
   
    Input.Position.x += sin(time) * 30;
    Input.Position.y += cos(time) * sign(Input.Position.y - 20);
    float variacion = Input.Position.y > 0 ? cos(time) * 1.1 : cos(time) * 0.9;
    Input.Position.y *= variacion;
    Input.Position.y += altura;

    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;

    return (Output);
}

//Pixel Shader
float4 ps_main(float3 Texcoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(diffuseMap, Texcoord);
    color.a = 0.7;
    return color;
}


technique WaveEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}