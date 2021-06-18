float oIntensity;
float oGlobalTime;

sampler TextureSampler : register(s0);
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 mid = float2(0.5, 0.5);
    
    float dist = distance(coords, mid);
    
    coords.x -= 0.002f * sin(oGlobalTime + coords.y * 25 + coords.x * 25) * oIntensity;
    coords.y -= 0.003f * sin(oGlobalTime + coords.y / 25 + coords.x * 50) * oIntensity;
    
    float4 Color = tex2D(TextureSampler, coords);
    
    Color.rgba -= dist * 0.75 * oIntensity;

    return Color;
}

technique BlackVignette
{
    pass BlackVignettePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}