#ifndef HUM_TOON_BASE_COLOR_INCLUDED
#define HUM_TOON_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

void HumCalcBaseColor(float2 uv, out half4 baseColor, out half3 outBaseMapColor)
{
    // NOTE:
    // outBaseMapColorはShadeの項目で使用する可能性があるため、outで渡す。
    // defineで分岐しない理由は、Base <=> Shade間で予期しずらい依存を持たせないようにするため。

    half4 baseMapColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    outBaseMapColor = baseMapColor.rgb;
    half4 tempBaseColor = baseMapColor * _BaseColor;

    baseColor.a = AlphaDiscard(tempBaseColor.a, _Cutoff);
    baseColor.rgb = AlphaModulate(tempBaseColor.rgb, baseColor.a);
}

#endif
