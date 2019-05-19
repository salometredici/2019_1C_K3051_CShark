
float time = 0;

struct VS_INPUT_SUELO
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_SUELO
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
    float Rayitas : TEXCOORD3;
    float Distancia : TEXCOORD4;
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

float4 calcularBlendRayos(float rayitas, float2 texcoord)
{
    float plano = 1 - rayitas;
    float4 colorTexturaOriginal = tex2D(diffuseMap, texcoord);
    float4 colorTexturaRayos = tex2D(diffuseMapRayos, texcoord);
    return colorTexturaOriginal * plano + colorTexturaRayos * rayitas;
}

VS_OUTPUT_SUELO vertex_suelo(VS_INPUT_SUELO Input)
{
    VS_OUTPUT_SUELO Output;
    float alturaSuperficie = 18000.0f;
    float proporcion = Input.Position.y / alturaSuperficie;
    float tamanioTile = 32.0f;
    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord * tamanioTile;

    //si esta debajo de las olas, muevo los rayitos del sol
    if (Input.Position.y < alturaSuperficie)
    {
        Output.Texcoord.x += sin(time) / tamanioTile;
        Output.Texcoord.y += cos(time) / tamanioTile;
    }

    Output.WorldPosition = mul(Input.Position, matWorld);
    Output.WorldNormal = mul(Input.Normal, matInverseTransposeWorld).xyz;
    Output.Rayitas = Input.Position.y < alturaSuperficie ? 0.8 : 0.15; //si estoy bajo agua veo MÁS los reflejitos
    Output.Distancia = mul(Input.Position, matWorldView).z;
    return Output;
}