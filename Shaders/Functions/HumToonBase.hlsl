#ifndef HUM_TOON_BASE_COLOR_INCLUDED
#define HUM_TOON_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

half4 CalcBaseColor(float2 uv, out half3 outBaseMapColor)
{
    // NOTE:
    // outBaseMapColorはShadeの項目で使用する可能性があるため、outで渡す。
    // defineで分岐しない理由は、Base <=> Shade間で予期しずらい依存を持たせないようにするため。

    half4 baseMapColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    outBaseMapColor = baseMapColor.rgb;
    half4 baseColor = baseMapColor * _BaseColor;

    baseColor.a = AlphaDiscard(baseColor.a, _Cutoff);
    baseColor.rgb = AlphaModulate(baseColor.rgb, baseColor.a);

    return baseColor;
}

#endif
