float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;
 
float multiply = 2;
float4 glowColor = float4(1, 0, 0, 1);

const static int FULLSIZE = 8;

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
    float4 Position : POSITION;
    float2 UV : TEXCOORD;
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

float4 PS_Main(PS_INPUT input) : COLOR0
{
	float4 color = (float4)0;
	float2 uvMod = float2(multiply, multiply) / TextureSize;
	
	for (int x = -FULLSIZE; x < FULLSIZE; x++)
	{
		for (int y = -FULLSIZE; y < FULLSIZE; y++)
		{
			color += tex2D(textureSampler, input.UV + (uvMod * float2(x, y)));
		}
	}

	color /= (FULLSIZE * 2) * (FULLSIZE * 2);
	color.r = color.a * glowColor.r;
	color.g = color.a * glowColor.g;
	color.b = color.a * glowColor.b;
	
	float4 colorT = tex2D(textureSampler, input.UV);
	
	color -= color * colorT.a;
	color += colorT * colorT.a;

    return color;
}
 
technique technique0
{
    pass p0
    {
        VertexShader = compile vs_3_0 VS_Main();
        PixelShader = compile ps_3_0 PS_Main();
    }
}