#ifndef HUM_TOON_SHADE_INCLUDED
#define HUM_TOON_SHADE_INCLUDED

#include "../ShaderLibrary/Func.hlsl"

half3 CalcShade(float2 uv, half3 color, float3 normalWS, float3 mainLightDirWS)
{
    half halfLambert = CalcHalfLambert(normalWS, mainLightDirWS);

    half firstShadeBorderPos = _FirstShadeBorderPos;
    half firstShadeBorderBlur = _FirstShadeBorderBlur;
    half smoothstepHalfLambert = smoothstep(
                                    firstShadeBorderPos - firstShadeBorderBlur,
                                    firstShadeBorderPos + firstShadeBorderBlur,
                                    halfLambert);
    half firstShade = OneMinus(smoothstepHalfLambert);

    // half3 firstShadeMap = SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv); // NOTE: 実装する場合は、use base map as first shade map 等を意識した実装にする
    half3 firstShadeColor = _FirstShadeColor;

    return lerp(color, color * firstShadeColor, firstShade); // NOTE: 暫定的に use base map as first shade mapのような挙動に
}

#endif
