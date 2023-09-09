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

    half3 firstShadeColor = _FirstShadeColor.rgb;
#ifdef _USE_FIRST_SHADE_MAP
    firstShadeColor *= SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv).rgb;
#else
    firstShadeColor *= color;
#endif

    return lerp(color, firstShadeColor, firstShade);
}

#endif
