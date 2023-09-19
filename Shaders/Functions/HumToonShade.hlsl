#ifndef HUM_TOON_SHADE_INCLUDED
#define HUM_TOON_SHADE_INCLUDED

#include "../ShaderLibrary/Func.hlsl"

half HumCalcShadeSmoothstep(half halfLambert, half ShadeBorderPos, half ShadeBorderBlur)
{
    half smoothstepHalfLambert = smoothstep(
                                    ShadeBorderPos - ShadeBorderBlur,
                                    ShadeBorderPos + ShadeBorderBlur,
                                    halfLambert);
    return OneMinus(smoothstepHalfLambert);
}

half HumCalcExShadeSmoothstep(half halfLambert, half shadeBorderPos, half shadeBorderBlur, half exShadeWidth)
{
    half startCenter = shadeBorderPos + exShadeWidth;
    half endCenter   = shadeBorderPos - exShadeWidth;

    // NOTE: blurはwidthを超えてはいけない(グラデが汚くなるため)
    shadeBorderBlur = shadeBorderBlur > exShadeWidth ? exShadeWidth : shadeBorderBlur;

    half start = smoothstep(
                    startCenter - shadeBorderBlur,
                    startCenter + shadeBorderBlur,
                    halfLambert);
    half end   = smoothstep(
                    endCenter - shadeBorderBlur,
                    endCenter + shadeBorderBlur,
                    halfLambert);

    return halfLambert > shadeBorderPos ? OneMinus(start) : end;
}

half3 MixFirstShade(float2 uv, half3 originalColor, half halfLambert)
{
#if defined(_USE_EX_FIRST_SHADE)
    half firstShade =  OneMinus(step(_FirstShadeBorderPos, halfLambert));
#else
    half firstShade = HumCalcShadeSmoothstep(halfLambert, _FirstShadeBorderPos, _FirstShadeBorderBlur);
#endif

    half3 firstShadeColor = _FirstShadeColor.rgb;
#ifdef _USE_FIRST_SHADE_MAP
    firstShadeColor *= SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv).rgb;
#else
    firstShadeColor *= originalColor;
#endif

    half3 finalFirstShadedColor;

    finalFirstShadedColor = lerp(originalColor, firstShadeColor, firstShade);
#if defined(_USE_EX_FIRST_SHADE)
    half exFirstShade = HumCalcExShadeSmoothstep(halfLambert, _FirstShadeBorderPos, _FirstShadeBorderBlur, _ExFirstShadeWidth);
    finalFirstShadedColor = lerp(finalFirstShadedColor, originalColor * _ExFirstShadeColor, exFirstShade);
#endif

    return finalFirstShadedColor;
}

half3 MixSecondShade(float2 uv, half3 originalColor, half halfLambert)
{
    half secondShade = HumCalcShadeSmoothstep(halfLambert, _SecondShadeBorderPos, _SecondShadeBorderBlur);

    half3 secondShadeColor = _SecondShadeColor.rgb;
#ifdef _USE_SECOND_SHADE_MAP
    secondShadeColor *= SAMPLE_TEXTURE2D(_SecondShadeMap, sampler_BaseMap, uv).rgb;
#else
    secondShadeColor *= originalColor;
#endif

    return lerp(originalColor, secondShadeColor, secondShade);
}

half3 MixShade(float2 uv, half3 originalColor, float3 normalWS, float3 mainLightDirWS)
{
    half3 finalShadedColor = originalColor;

    half halfLambert = CalcHalfLambert(normalWS, mainLightDirWS);

#if defined(_USE_FIRST_SHADE)
    finalShadedColor = MixFirstShade(uv, finalShadedColor, halfLambert);
#endif

#if defined(_USE_SECOND_SHADE)
    finalShadedColor = MixSecondShade(uv, finalShadedColor, halfLambert);
#endif

    return finalShadedColor;
}

#endif
