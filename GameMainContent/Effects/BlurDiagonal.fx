float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;
 
float inSize;
Texture Texture;
 
sampler2D textureSampler = sampler_state 
{
    Texture = (Texture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VS_INPUT
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};
 
struct PS_INPUT
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

PS_INPUT VS_Main(VS_INPUT input)
{
    PS_INPUT output = (PS_INPUT)0;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.UV = input.UV;
 
    return output;
}

float4 PS_Horizontal(float2 uv, bool plus, float pSize)
{
	float size = pSize;
	if (!plus) size = -size;	
	
    float4 color1 = tex2D(textureSampler, uv + float2(0, 0));
    float4 color2 = tex2D(textureSampler, uv + float2(size*1, size*1));
    float4 color3 = tex2D(textureSampler, uv + float2(size*2, size*2));
    float4 color4 = tex2D(textureSampler, uv + float2(size*3, size*3));
    
	float4 color = color1 * 0.1;
	color += color2 * 0.3;
	color += color3 * 0.3;
	color += color4 * 0.3;

	return color;
}

float4 PS_Vertical(float2 uv, bool plus, float pSize)
{
	float size = pSize;
	if (!plus) size = -size;
	
    float4 color1 = tex2D(textureSampler, uv + float2(0, 0));
    float4 color2 = tex2D(textureSampler, uv + float2(-size*1, size*1));
    float4 color3 = tex2D(textureSampler, uv + float2(-size*2, size*2));
    float4 color4 = tex2D(textureSampler, uv + float2(-size*3, size*3));
    
	float4 color = color1 * 0.1;
	color += color2 * 0.3;
	color += color3 * 0.3;
	color += color4 * 0.3;

	return color;	
}

float4 PS_Main(PS_INPUT input) : COLOR0
{
    float4 color1 = PS_Horizontal(input.UV, true, inSize);
    float4 color2 = PS_Horizontal(input.UV, false, inSize);
    float4 color3 = PS_Vertical(input.UV, true, inSize);
    float4 color4 = PS_Vertical(input.UV, false, inSize);
    	
	float4 color = color1 * 0.25;
	color += color2 * 0.25;
	color += color3 * 0.25;
	color += color4 * 0.25;
	
	color.a -= inSize * 50;
	color.rgb -= inSize * 25;

    return color;
}
 
technique technique0
{
    pass p0
    {
        VertexShader = compile vs_2_0 VS_Main();
        PixelShader = compile ps_2_0 PS_Main();
    }
}