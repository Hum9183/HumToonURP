#ifndef HUM_TOON_RIM_LIGHT_INCLUDED
#define HUM_TOON_RIM_LIGHT_INCLUDED

#include "../../ShaderLibrary/Func.hlsl"

half3 HumCalcRimLightColor(float2 uv, float3 normalWS, float3 viewDirWS, half3 mainLightColor)
{
    // TODO:
    // ・ブレンド方法(乗算etc.)
    // ・MapのAチャンネルでのMask
    // ・Mask

    // Rim
    half NdotV = dot(normalWS, viewDirWS);
    half rim = HumBlurStep(_RimLightBorderPos, _RimLightBorderBlur, NdotV);
    rim = OneMinus(rim);

    // Color
    half3 rimLightColor = _RimLightColor;
#if defined(_HUM_USE_RIM_LIGHT_MAP)
    rimLightColor *= SAMPLE_TEXTURE2D(_RimLightMap, sampler_BaseMap, uv).rgb;
#endif

    // Composite
    half3 finalRimLightColor = rim * rimLightColor;
    finalRimLightColor = lerp(finalRimLightColor, finalRimLightColor * mainLightColor, _RimLightMainLightEffectiveness);

    return finalRimLightColor * _RimLightIntensity;
}

#endif
