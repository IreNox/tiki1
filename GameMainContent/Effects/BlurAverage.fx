float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;

float multiply = 2;
 
const static int fullSize = 8;

float2 TextureSize;
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
    float2 UV[fullSize] : TEXCOORD;
};

PS_INPUT VS_Main(VS_INPUT input)
{
    PS_INPUT output = (PS_INPUT)0;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	
	float2 uvSize = float2(multiply, multiply) / TextureSize;
	
	for (int i = 0; i < fullSize / 2; i++)
	{
		output.UV[i] = input.UV + float2(uvSize.x*(i - (fullSize / 4)), 0);
	}
	
	for (int i = fullSize / 2; i < fullSize; i++)
	{
		output.UV[i] = input.UV + float2(0, uvSize.y*(i - (fullSize * 0.75)));
	}	
 
    return output;
}

float4 PS_Main(PS_INPUT input) : COLOR0
{
	float4 color = (float4)0;
	
	for (int i = 0; i < fullSize; i++)
	{
		color += tex2D(textureSampler, input.UV[i]);
	}
	
	color /= fullSize;
	
	
	//color.a -= inSize * 50;
	//color.rgb -= inSize * 25;

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