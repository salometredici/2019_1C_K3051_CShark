//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float3x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

//Variables de las luces
float3 coloresLuces[10];
float3 posicionesLuces[10];
float intensidadesLuces[10];
float atenuacionesLuces[10];

//Cantidad real de luces 
int cantidadLuces;

//Propiedades del material
float4 colorDifuso;
float3 colorEmisivo;
float3 colorAmbiente;
float3 colorEspecular;
float exponenteEspecular;

//Posicion de la camara
float4 posicionCamara;

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

texture texLightMap;
sampler2D lightMap = sampler_state
{
    Texture = (texLightMap);
};

struct Luz
{
    float3 Color;
    float3 Posicion;
    float Intensidad;
    float Atenuacion;
};

struct VS_INPUT_DIFFUSE_MAP
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_DIFFUSE_MAP
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};

struct PS_DIFFUSE_MAP
{
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};

Luz getLuz(int i)
{
    Luz luz;
    luz.Color = coloresLuces[i];
    luz.Atenuacion = atenuacionesLuces[i];
    luz.Intensidad = intensidadesLuces[i];
    luz.Posicion = posicionesLuces[i];
    return luz;
};

VS_OUTPUT_DIFFUSE_MAP vertex_shader(VS_INPUT_DIFFUSE_MAP input)
{
    VS_OUTPUT_DIFFUSE_MAP output;
    output.Position = mul(input.Position, matWorldViewProj);
    output.Texcoord = input.Texcoord;
    output.WorldPosition = mul(input.Position, matWorld);
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;
    return output;
}

float4 pixel_shader(PS_DIFFUSE_MAP input) : COLOR0
{
    float3 Nn = normalize(input.WorldNormal);
    float3 luzEspecular = 0;
    float3 luzAmbiente = 0;
    float3 luzDifusa = colorEmisivo;

    for (int i = 0; i < cantidadLuces; i++)
    {
        Luz luz = getLuz(i);

        float3 Ln = normalize(luz.Posicion - input.WorldPosition);
        float3 Hn = normalize(posicionCamara.xyz - 2 * input.WorldPosition + luz.Posicion);
        float3 n_dot_l = max(0.0, dot(Nn, Ln));
        float3 n_dot_h = max(0.0, dot(Nn, Hn));

        float distanciaAtenuada = length(luz.Posicion - input.WorldPosition) * luz.Atenuacion;
        float intensidad = luz.Intensidad / distanciaAtenuada;
        
        luzAmbiente += intensidad * luz.Color;
        luzDifusa += intensidad * luz.Color * n_dot_l;
        luzEspecular += intensidad * luz.Color * pow(n_dot_h, exponenteEspecular);
    }

    luzEspecular *= colorEspecular;
    luzAmbiente *= colorAmbiente;
    luzDifusa *= colorDifuso;

    float3 colorTexel = tex2D(diffuseMap, input.Texcoord);

    return float4(saturate(colorEmisivo + luzDifusa + luzAmbiente) * colorTexel + luzEspecular, colorDifuso.a);
}

technique Iluminado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_shader();
        PixelShader = compile ps_3_0 pixel_shader();
    }
}