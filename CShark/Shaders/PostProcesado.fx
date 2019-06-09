float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection

struct VS_INPUT_DEFAULT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_DEFAULT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

struct PS_INPUT_DEFAULT
{
    float2 Texcoord : TEXCOORD0;
};

texture render_target2D;
sampler RenderTarget = sampler_state
{
    Texture = (render_target2D);
    MipFilter = NONE;
    MinFilter = NONE;
    MagFilter = NONE;
};

texture textura_flare;
sampler sampler_flare = sampler_state
{
    Texture = (textura_flare);
};

texture textura_casco;
sampler sampler_casco = sampler_state
{
    Texture = (textura_casco);
};

float4 posicionSol;
float4 posicionPlayer;
bool mostrarCasco;

VS_OUTPUT_DEFAULT vs_default(VS_INPUT_DEFAULT Input)
{
    VS_OUTPUT_DEFAULT Output;
    Output.Position = float4(Input.Position.xy, 0, 1);
    Output.Texcoord = Input.Texcoord;
    return (Output);
}
 
float4 ps_general(in float2 Texcoord : TEXCOORD0) : COLOR0
{
    float4 colorEscena = tex2D(RenderTarget, Texcoord);
    float4 colorCasco = tex2D(sampler_casco, Texcoord);
    //float4 colorLens = tex2D(sampler_flare, Texcoord);
    float4 colorFinal = colorEscena;
    if (mostrarCasco)
        colorFinal = colorCasco.g < 0.5f 
        ? colorCasco 
        : float4(1, 1, 1, 1) * (1 - colorCasco.g) + colorEscena; //bordecito suave
    return colorFinal;
}
 
technique General
{
    pass p0
    {
        VertexShader = compile vs_3_0 vs_default();
        PixelShader = compile ps_3_0 ps_general();
    }
}