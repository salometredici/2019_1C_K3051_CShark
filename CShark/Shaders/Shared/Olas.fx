struct VS_INPUT_OLAS
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT_OLAS
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float Alpha : TEXCOORD1;
    float Distancia : TEXCOORD2;
};

//cuando mas lejos esté del player, mas opaco va a ser, para que no vea todo..
float calcularAlpha(float3 position)
{
    float distancia = distance(position, posicionCamara.xyz);
    float maximaVision = 50000; //donde quiero que empiece a ser 1
    return clamp(1 / maximaVision * distancia, 0.6, 1);
}

VS_OUTPUT_OLAS vs_olas(VS_INPUT_OLAS Input)
{
    VS_OUTPUT_OLAS Output;
    
    float altura = 18000;
    float tamanioTextura = 256.0f;
    float tamanioTile = tamanioTextura / 32;
    Input.Position.x += sin(time) * 30;
    Input.Position.y += cos(time) * sign(Input.Position.y - 20);
    Input.Position.z += sin(time);
    float variacion = Input.Position.y > 0 ? cos(time) * 2.5 : cos(time) * 1.25;
    Input.Position.y *= variacion;
    Input.Position.y += altura;

    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord * tamanioTile;
    Output.Alpha = calcularAlpha(Input.Position.xyz);
    Output.Distancia = mul(Input.Position, matWorldView).z;

    return (Output);
}