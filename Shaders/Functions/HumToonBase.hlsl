#ifndef HUM_TOON_BASE_COLOR_INCLUDED
#define HUM_TOON_BASE_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

void HumCalcBaseColor(float2 uv, SurfaceData surfaceData, out half4 outBaseColor, out half3 outBaseColorWithoutBaseColor)
{
    // NOTE: baseColorWithoutBaseColor
    // _BaseColorが適用されていないBaseColor。
    // Shadeの項目で使用する可能性があるため、outで渡す。
    // defineで分岐しない理由は、Base <=> Shade間で予期しずらい依存を持たせないようにするため。

    half4 baseColor = half4(surfaceData.albedo, surfaceData.alpha);
    outBaseColorWithoutBaseColor = baseColor;
    outBaseColor = baseColor * _BaseColor;
}

#endif
