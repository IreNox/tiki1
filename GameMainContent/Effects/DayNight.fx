float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;

Texture tDiffuse;
Texture tLightMap;

float time; // : TIME;

float PI = 3.14159265f;
float twoPi = 6.28318530f;
float halfPi = 1.57079632f;

//float lightDirection;
//float2 pos;
//float pointLightRange = 3;

sampler sDiffuse = sampler_state { texture = <tDiffuse>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};
sampler sLightMap = sampler_state { texture = <tLightMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

struct VS_INPUT
{
	float4 Position	: POSITION;
	float2 UV		: TEXCOORD0;	
};

struct PS_INPUT
{
    float4 Position	: POSITION0;
	float2 UV		: TEXCOORD0;
};

float4 AddColor(float4 c1, float4 c2)
{
	float4 o = 0;
	
	o.r = (c1.r * c1.a) + (c2.r * c2.a);
	o.g = (c1.g * c1.a) + (c2.g * c2.a);
	o.b = (c1.b * c1.a) + (c2.b * c2.a);
	o.a = (c1.a + c2.b) / 2;
	
	return o;
}

PS_INPUT VertexShaderFunction(VS_INPUT input)
{
    PS_INPUT output = (PS_INPUT) 0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;

    return output;
}

float4 PixelShaderFunction(PS_INPUT input) : COLOR0
{
	float time2 = 1 - time;
	
	float tD = (sin(time2 / PI) + 1) / 2;
	float tN = -1 + tD;
	
	float2 p = {
		(sin(time2 / PI) / halfPi) + 0.5f,
		(cos(time2 / PI) / halfPi) + 0.5f
	};
	
	//float lightDirection = ;
	//float attenuation = saturate(1 - dot(lightDirection / pointLightRange, lightDirection / pointLightRange));

	float4 cLM = tex2D(sLightMap, input.UV) * distance(input.UV, p) * tN * 0.75f;
	float4 cDiffuse = tex2D(sDiffuse, input.UV);
	
	cLM.a = tN;
	cDiffuse.rgba *= tD;

	float4 w = { 1, 1, 1, 1 };
	float4 b = { cDiffuse.r, cDiffuse.g, cDiffuse.b, tD };
	
	float4 output = AddColor(cDiffuse, cLM);
	output.a = 1;
	
    return output;
}

/*technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}*/

technique technique0 {
	pass p0 {
		CullMode = None;
		VertexShader = compile vs_3_0 VertexShaderFunction(); //mainVS();
		PixelShader = compile ps_3_0 PixelShaderFunction(); //mainPS();
	}
}
