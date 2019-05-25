/*
* Shaders Post Procesado
*/

/**************************************************************************************/
/* DEFAULT */
/**************************************************************************************/

//Input del Vertex Shader
struct VS_INPUT_DEFAULT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_DEFAULT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

//Vertex Shader
VS_OUTPUT_DEFAULT vs_default(VS_INPUT_DEFAULT Input)
{
    VS_OUTPUT_DEFAULT Output;

	//Proyectar posicion
    Output.Position = float4(Input.Position.xy, 0, 1);

	//Las Texcoord quedan igual
    Output.Texcoord = Input.Texcoord;

    return (Output);
}

//Textura del Render target 2D
texture render_target2D;

sampler RenderTarget = sampler_state
{
    Texture = (render_target2D);
    MipFilter = NONE;
    MinFilter = NONE;
    MagFilter = NONE;
};

//Input del Pixel Shader
struct PS_INPUT_DEFAULT
{
    float2 Texcoord : TEXCOORD0;
};

// Textura casco
texture casco_textura;

sampler sampler_casco = sampler_state
{
    Texture = (casco_textura);
};

//Pixel Shader del Casco
float4 ps_casco(PS_INPUT_DEFAULT Input) : COLOR0
{
	//Obtener color segun textura
    float4 colorOriginal = tex2D(RenderTarget, Input.Texcoord);

	//Obtener color de textura de alarma, escalado por un factor
    float4 colorCasco = tex2D(sampler_casco, Input.Texcoord);
	float4 colorFinal = colorCasco;
	if(colorCasco.g > 0.7f && colorCasco.r < 0.5f){
		colorFinal = colorOriginal;
	}
	
    return colorFinal;
}

technique CascoTechnique
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_default();
        PixelShader = compile ps_3_0 ps_casco();
    }
}