#ifndef HT_RIM_LIGHT_INCLUDED
#define HT_RIM_LIGHT_INCLUDED

#include "..\..\ShaderLibrary\HTUtils.hlsl"

half3 HTCalcRimLightColor(float2 uv, float3 normalWS, float3 viewDirWS, half3 mainLightColor)
{
    // TODO:
    // ・ブレンド方法(乗算etc.)
    // ・MapのAチャンネルでのMask
    // ・Mask

    // Rim
    half NdotV = dot(normalWS, viewDirWS);
    half rim = HTBlurStep(_RimLightBorderPos, _RimLightBorderBlur, NdotV);
    rim = HTOneMinus(rim);

    // Color
    half3 rimLightColor = _RimLightColor;
#if defined(_HT_USE_RIM_LIGHT_MAP)
    rimLightColor *= SAMPLE_TEXTURE2D(_RimLightMap, sampler_BaseMap, uv).rgb;
#endif

    // Composite
    half3 finalRimLightColor = rim * rimLightColor;
    finalRimLightColor = lerp(finalRimLightColor, finalRimLightColor * mainLightColor, _RimLightMainLightEffectiveness);

    return finalRimLightColor * _RimLightIntensity;
}

#endif
