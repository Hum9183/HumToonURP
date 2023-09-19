#ifndef HUM_TOON_TEMP_INCLUDED
#define HUM_TOON_TEMP_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

half4 CalcBaseColor(float2 uv)
{
    half4 baseMapColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    half3 baseColor = baseMapColor.rgb * _BaseColor.rgb;
    half alpha = baseMapColor.a * _BaseColor.a;

    alpha = AlphaDiscard(alpha, _AlphaCutoff);
    baseColor = AlphaModulate(baseColor, alpha);

    return float4(baseColor, alpha);
}

#include "../../../ShaderLibrary/Func.hlsl"

half3 CalcShade(half3 color, float3 normalWS, float3 lightDir)
{
    float halfLambert = HalfLambert(normalWS, lightDir);
    // TODO: step他
    return halfLambert.xxx;
}

#endif
