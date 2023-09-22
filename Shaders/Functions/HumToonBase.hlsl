#ifndef HUM_TOON_BASE_COLOR_INCLUDED
#define HUM_TOON_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

half4 CalcBaseColor(float2 uv
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , out half3 outBaseMapColor
#endif
)
{
    half4 baseMapColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
#if !defined(_HUM_USE_FIRST_SHADE_MAP) || !defined(_HUM_USE_SECOND_SHADE_MAP) || defined(_HUM_USE_EX_FIRST_SHADE)
    outBaseMapColor = baseMapColor.rgb;
#endif
    half4 baseColor = baseMapColor * _BaseColor;

    baseColor.a = AlphaDiscard(baseColor.a, _Cutoff);
    baseColor.rgb = AlphaModulate(baseColor.rgb, baseColor.a);

    return baseColor;
}

#endif
