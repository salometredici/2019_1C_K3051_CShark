//variables niebla
float4 colorNiebla;
float distanciaInicio;
float distanciaFin;
float densidad;

struct Niebla
{
    float3 Color;
    float DistanciaInicio;
    float DistanciaFin;
    float Densidad;
};

bool nublado(float distancia)
{
    return distancia > distanciaInicio && distanciaFin > distancia;
}

float4 calcularNiebla(float distancia, float4 color)
{
    if (nublado(distancia))
    {
        float total = distanciaFin - distanciaInicio;
        float resto = distancia - distanciaInicio;
        float niebla = 1 - resto / total;
        color = (1 - niebla) * colorNiebla + color * niebla;
    }
    return distancia > distanciaFin ? colorNiebla : color;
}