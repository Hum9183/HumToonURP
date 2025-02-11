#ifndef HT_SHADE_INCLUDED
#define HT_SHADE_INCLUDED

#include "..\HTPredefine.hlsl"
#include "..\..\ShaderLibrary\HTUtils.hlsl"

half HTCalcExShadeSmoothstep(half halfLambert, half shadeBorderPos, half shadeBorderBlur, half exShadeWidth)
{
    half startCenter = shadeBorderPos + exShadeWidth;
    half endCenter   = shadeBorderPos - exShadeWidth;

    // NOTE: blurはwidthを超えてはいけない(グラデが汚くなるため)
    shadeBorderBlur = shadeBorderBlur > exShadeWidth ? exShadeWidth : shadeBorderBlur;

    half start = HTBlurStep(startCenter, shadeBorderBlur, halfLambert);
    half end   = HTBlurStep(endCenter,   shadeBorderBlur, halfLambert);

    return halfLambert > shadeBorderPos ? HTOneMinus(start) : end;
}

half3 HTMixFirstShade(float2 uv, half3 baseColor, half halfLambert, half shadowAttenuation
#if NOT(defined(_HT_USE_FIRST_SHADE_MAP)) || defined(_HT_USE_EX_FIRST_SHADE)
    , half3 baseMapColorOnly
#endif
)
{
#if defined(_HT_USE_EX_FIRST_SHADE)
    // NOTE: Exを使用する場合は、FirstShadeの境界がExで上書きされるため、smoothstepをしないほうが綺麗になる
    half firstShade =  step(_FirstShadeBorderPos, halfLambert);
#else
    half firstShade = HTBlurStep(_FirstShadeBorderPos, _FirstShadeBorderBlur, halfLambert);
#endif
    firstShade *= shadowAttenuation;

    // First Shade Color
#if defined(_HT_USE_FIRST_SHADE_MAP)
    half3 firstShadeMapColor = SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 firstShadeMapColor = baseMapColorOnly;
#endif
    half3 firstShadeColor = firstShadeMapColor * _FirstShadeColor.rgb;

    // Composite
    half3 finalFirstShadedColor = lerp(firstShadeColor, baseColor, firstShade);

#if defined(_HT_USE_EX_FIRST_SHADE)
    // Ex
    half exFirstShade = HTCalcExShadeSmoothstep(halfLambert, _FirstShadeBorderPos, _FirstShadeBorderBlur, _ExFirstShadeWidth);
    half3 exFirstShadeColor = baseMapColorOnly * _ExFirstShadeColor;
    finalFirstShadedColor = lerp(finalFirstShadedColor, exFirstShadeColor, exFirstShade);
#endif

    return finalFirstShadedColor;
}

half3 HTMixSecondShade(float2 uv, half3 originalColor, half halfLambert
#if NOT(defined(_HT_USE_SECOND_SHADE_MAP))
    , half3 baseMapColorOnly
#endif
)
{
    half secondShade = (HTBlurStep(_SecondShadeBorderPos, _SecondShadeBorderBlur, halfLambert));

    // Second Shade Color
#ifdef _HT_USE_SECOND_SHADE_MAP
    half3 secondShadeMapColor = SAMPLE_TEXTURE2D(_SecondShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 secondShadeMapColor = baseMapColorOnly;
#endif
    half3 secondShadeColor = secondShadeMapColor * _SecondShadeColor.rgb;

    return lerp(secondShadeColor, originalColor, secondShade);
}

half3 HTMixPosAndBlurShade(float2 uv, half3 baseColor, half halfLambert, half shadowAttenuation
#ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
    , half3 baseMapColorOnly
#endif
)
{
    half3 finalShadedColor = baseColor;

#if defined(_HT_USE_FIRST_SHADE)
    finalShadedColor = HTMixFirstShade(uv, finalShadedColor, halfLambert, shadowAttenuation
    #if NOT(defined(_HT_USE_FIRST_SHADE_MAP)) || defined(_HT_USE_EX_FIRST_SHADE)
        , baseMapColorOnly
    #endif
    );
#endif

#if defined(_HT_USE_SECOND_SHADE)
    finalShadedColor = HTMixSecondShade(uv, finalShadedColor, halfLambert
    #if NOT(defined(_HT_USE_SECOND_SHADE_MAP))
        , baseMapColorOnly
    #endif
    );
#endif

    return finalShadedColor;
}


half3 HTMixRampShade(float2 uv, half3 baseColor, half halfLambert)
{
    half3 finalShadedColor = baseColor;

#if defined(_HT_USE_RAMP_SHADE)
    // NOTE: halfLambertの型はhalfではなくfloatのほうが良いかもしれない
    half anyValueIsAcceptable = 0;
    half2 rampUV = half2(halfLambert, anyValueIsAcceptable);
    half3 rampMapColor = _RampShadeMap.Sample(hum_sampler_linear_clamp, rampUV).rgb;
    finalShadedColor *= rampMapColor;
#endif

    return finalShadedColor;
}

half HTCalcShadeHalfLambert(float2 uv, float3 normalWS, float3 mainLightDirWS)
{
    half halfLambert = HTCalcHalfLambert(normalWS, mainLightDirWS);
#if defined(_HT_USE_SHADE_CONTROL_MAP)
    half shadeControl = SAMPLE_TEXTURE2D(_ShadeControlMap, sampler_BaseMap, uv).r;
    shadeControl = lerp(ONE, shadeControl, _ShadeControlMapIntensity);
    halfLambert *= shadeControl;
#endif

    return halfLambert;
}

// TODO:
// - shadowAttenuationをexShadeに対応させる(根本的な構造を見直す必要がありそうな気配...)
half3 HTMixShadeColor(float2 uv, half3 baseColor, float3 normalWS, float3 mainLightDirWS, half shadowAttenuation
#ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
    , half3 baseMapColorOnly
#endif
)
{
    half halfLambert = HTCalcShadeHalfLambert(uv, normalWS, mainLightDirWS);

    half3 finalShadedColor = 0;

#if defined(_HT_SHADE_MODE_POS_AND_BLUR)
    finalShadedColor = HTMixPosAndBlurShade(uv, baseColor, halfLambert, shadowAttenuation
    #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    );
#endif

#if defined(_HT_SHADE_MODE_RAMP)
    finalShadedColor = MixRampShade(uv, baseColor, halfLambert);
#endif

    return finalShadedColor;
}

#endif
