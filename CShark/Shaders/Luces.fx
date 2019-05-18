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

struct Luz
{
    float3 Color;
    float3 Posicion;
    float Intensidad;
    float Atenuacion;
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
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};

struct PS_INPUT
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

float4 calcularLuces(float3 worldNormal, float3 worldPosition, float3 texel)
{
    float3 Nn = normalize(worldNormal);
    float3 luzEspecular = 0;
    float3 luzAmbiente = 0;
    float3 luzDifusa = colorEmisivo;

    for (int i = 0; i < cantidadLuces; i++)
    {
        Luz luz = getLuz(i);

        float3 Ln = normalize(luz.Posicion - worldPosition);
        float3 Hn = normalize(posicionCamara.xyz - 2 * worldPosition + luz.Posicion);
        float3 n_dot_l = max(0.0, dot(Nn, Ln));
        float3 n_dot_h = max(0.0, dot(Nn, Hn));

        float distanciaAtenuada = length(luz.Posicion - worldPosition) * luz.Atenuacion;
        float intensidad = luz.Intensidad / distanciaAtenuada;
        
        luzAmbiente += intensidad * luz.Color;
        luzDifusa += intensidad * luz.Color * n_dot_l;
        luzEspecular += intensidad * luz.Color * pow(n_dot_h, exponenteEspecular);
    }

    luzEspecular *= colorEspecular;
    luzAmbiente *= colorAmbiente;
    luzDifusa *= colorDifuso;
       
    return float4(saturate(colorEmisivo + luzDifusa + luzAmbiente) * texel + luzEspecular, colorDifuso.a);
}

VS_OUTPUT vertex_iluminado(VS_INPUT input)
{
    VS_OUTPUT output;
    output.Position = mul(input.Position, matWorldViewProj);
    output.Texcoord = input.Texcoord;
    output.WorldPosition = mul(input.Position, matWorld);
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;
    return output;
}

float4 pixel_iluminado(PS_INPUT input) : COLOR0
{
    float3 colorTexel = tex2D(diffuseMap, input.Texcoord);
    return calcularLuces(input.WorldNormal, input.WorldPosition, colorTexel);
}

//simplemente iluminados
technique Iluminado
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_iluminado();
        PixelShader = compile ps_3_0 pixel_iluminado();
    }
}

struct VS_OUTPUT_RAYOS
{
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
    float Rayitas : TEXCOORD3;
    float4 Position : POSITION0;
};

texture texRayosSol;
sampler2D diffuseMapRayos = sampler_state
{
    Texture = (texRayosSol);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

float time = 0;

VS_OUTPUT_RAYOS vertex_iluminado_rayos(VS_INPUT Input)
{
    VS_OUTPUT_RAYOS Output;
    float alturaSuperficie = 18000.0f;
    float proporcion = Input.Position.y / alturaSuperficie;
    float tamanioTile = 0;
    /*if (0.6 < proporcion && proporcion <= 1)
        tamanioTile = 256.0f;
    else if (0.2 < proporcion && proporcion <= 0.6)
        tamanioTile = 128.0f;
    else if (proporcion <= 0.2)*/
        tamanioTile = 32.0f;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord * tamanioTile;
    Output.Texcoord.x += sin(time) / 32;
    Output.Texcoord.y += cos(time) / 32;
    Output.WorldPosition = mul(Input.Position, matWorld);
    Output.WorldNormal = mul(Input.Normal, matInverseTransposeWorld).xyz;
    Output.Rayitas = Input.Position.y < alturaSuperficie ? 0.8 : 0.15; //si estoy bajo agua veo MÁS los reflejitos
    return Output;
}

float4 pixel_iluminado_rayos(VS_OUTPUT_RAYOS Input) : COLOR0
{
    float plano = 1 - Input.Rayitas;
    float3 color1 = tex2D(diffuseMap, Input.Texcoord).rgb;
    float3 color2 = tex2D(diffuseMapRayos, Input.Texcoord).rgb;
    float3 colorTexel = color1 * plano + color2 * Input.Rayitas;
    //el texel es el que calculo haciendo el blend entre las dos texturas
    return calcularLuces(Input.WorldNormal, Input.WorldPosition, colorTexel);
}

//arena iluminada y un blend de arena simple + arena con rayos solares (bajo agua)
technique Iluminado_Rayos_Sol
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vertex_iluminado_rayos();
        PixelShader = compile ps_3_0 pixel_iluminado_rayos();
    }
}