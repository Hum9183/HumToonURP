#ifndef HUM_TOON_BASE_COLOR_INCLUDED
#define HUM_TOON_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

half4 CalcBaseColor(float2 uv)
{
    half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    half4 baseColor = baseMap * _BaseColor;

    baseColor.a = AlphaDiscard(baseColor.a, _Cutoff);
    baseColor.rgb = AlphaModulate(baseColor.rgb, baseColor.a);

    return baseColor;
}

#endif
