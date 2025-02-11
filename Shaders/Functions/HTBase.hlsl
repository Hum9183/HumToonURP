#ifndef HT_BASE_COLOR_INCLUDED
#define HT_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

void HTCalcBaseColor(float2 uv, SurfaceData surfaceData, out half4 outBaseColor, out half3 outBaseMapColorOnly)
{
    outBaseColor = half4(surfaceData.albedo, surfaceData.alpha);

    // NOTE: baseMapColorOnly
    // BaseMapをサンプリングしただけの色。_BaseColorは乗算されていない。
    // Shadeの項目で使用する可能性があるため、outで渡す。
    // defineで分岐しない理由は、Base <=> Shade間で予期しずらい依存を持たせないようにするため。
    outBaseMapColorOnly = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).rgb;
    outBaseMapColorOnly = AlphaModulate(outBaseMapColorOnly, surfaceData.alpha);
}

#endif
