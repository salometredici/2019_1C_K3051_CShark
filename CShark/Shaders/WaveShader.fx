//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float time = 0;
float4 posicionJugador;

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
    float Alpha : TEXCOORD1;
};

//cuando mas lejos esté del player, mas opaco va a ser, para que no vea todo..
float calcularAlpha(float3 position)
{
    float distancia = distance(position, posicionJugador.xyz);
    float maximaVision = 50000; //donde quiero que empiece a ser 1
    return clamp(1 / maximaVision * distancia, 0.6, 1);
}

VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;
    
    float altura = 18000;
    float tamanioTextura = 256.0f;
    float tamanioTile = tamanioTextura / 16;
    Input.Position.x += sin(time) * 30;
    Input.Position.y += cos(time) * sign(Input.Position.y - 20);
    Input.Position.z += sin(time);
    float variacion = Input.Position.y > 0 ? cos(time) * 2.5 : cos(time) * 1.25;
    Input.Position.y *= variacion;
    Input.Position.y += altura;

    Output.Position = mul(Input.Position, matWorldViewProj);
    Output.Texcoord = Input.Texcoord * tamanioTile;
    Output.Alpha = calcularAlpha(Input.Position.xyz);

    return (Output);
}



//despues ver si puedo agregar luces a las olas para que tengan brillito y eso..
//pero alta paja :)
float4 ps_main(VS_OUTPUT Input) : COLOR0
{
    float3 color = tex2D(diffuseMap, Input.Texcoord).rgb;
    return float4(color, Input.Alpha);
}


technique WaveEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}