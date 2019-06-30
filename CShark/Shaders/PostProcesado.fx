#include <Shared/Common.fx>

float KLum = 1; // factor de luminancia

texture g_RenderTarget;
sampler RenderTarget =
sampler_state
{
    Texture = <g_RenderTarget>;
    ADDRESSU = CLAMP;
    ADDRESSV = CLAMP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

texture g_GlowMap;
sampler GlowMap =
sampler_state
{
    Texture = <g_GlowMap>;
    ADDRESSU = CLAMP;
    ADDRESSV = CLAMP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 Norm : TEXCOORD1; // Normales
    float3 Pos : TEXCOORD2; // Posicion real 3d
};

VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord;
    Output.Pos = mul(Input.Position, matWorld).xyz;
    Output.Norm = normalize(mul(Input.Normal, matWorld));
    return (Output);
}

float4 ps_main(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord.xy);
    fvBaseColor.rgb *= KLum;
    return fvBaseColor;
}

technique DefaultTechnique
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}

void VSCopy(float4 vPos : POSITION, float2 vTex : TEXCOORD0, out float4 oPos : POSITION, out float2 oScreenPos : TEXCOORD0)
{
    oPos = vPos;
    oScreenPos = vTex;
    oPos.w = 1;
}

static const int kernel_r = 6;
static const int kernel_size = 13;
static const float Kernel[kernel_size] =
{
    0.002216, 0.008764, 0.026995, 0.064759, 0.120985, 0.176033, 0.199471, 0.176033, 0.120985, 0.064759, 0.026995, 0.008764, 0.002216,
};

void BlurH(float2 screen_pos : TEXCOORD0, out float4 Color : COLOR)
{
    Color = 0;
    for (int i = 0; i < kernel_size; ++i)
        Color += tex2D(RenderTarget, screen_pos + float2((float) (i - kernel_r) / screen_dx, 0)) * Kernel[i];
    Color.a = 1;
}

void BlurV(float2 screen_pos : TEXCOORD0, out float4 Color : COLOR)
{
    Color = 0;
    for (int i = 0; i < kernel_size; ++i)
        Color += tex2D(RenderTarget, screen_pos + float2(0, (float) (i - kernel_r) / screen_dy)) * Kernel[i];
    Color.a = 1;
}

technique GaussianBlurSeparable
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSCopy();
        PixelShader = compile ps_3_0 BlurH();
    }
    pass Pass_1
    {
        VertexShader = compile vs_3_0 VSCopy();
        PixelShader = compile ps_3_0 BlurV();
    }
}

float4 PSDownFilter4(in float2 Tex : TEXCOORD0) : COLOR0
{
    float4 Color = 0;
    for (int i = 0; i < 4; i++)
        for (int j = 0; j < 4; j++)
            Color += tex2D(RenderTarget, Tex + float2((float) i / screen_dx, (float) j / screen_dy));
    return Color / 16;
}

technique DownFilter4
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSCopy();
        PixelShader = compile ps_3_0 PSDownFilter4();
    }
}

texture textura_casco;
sampler sampler_casco = sampler_state
{
    Texture = (textura_casco);
};

bool mostrarCasco;

float4 ps_final(in float2 Tex : TEXCOORD0, in float2 vpos : VPOS) : COLOR0
{
    float4 ColorBase = tex2D(RenderTarget, Tex);
    float4 ColorGlow = tex2D(GlowMap, Tex + float2((float) 16 / screen_dx, (float) 16 / screen_dy));

    float4 colorCasco = tex2D(sampler_casco, Tex);

    float4 colorFinal = ColorBase + ColorGlow;
    if (mostrarCasco)
        colorFinal = colorCasco.g < 0.5f
        ? colorCasco
        : float4(1, 1, 1, 1) * (1 - colorCasco.g) + colorFinal; //bordecito suave
    return colorFinal;
}

technique Final
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSCopy();
        PixelShader = compile ps_3_0 ps_final();
    }
}
