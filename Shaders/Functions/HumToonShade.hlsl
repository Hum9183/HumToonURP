#ifndef HUM_TOON_SHADE_INCLUDED
#define HUM_TOON_SHADE_INCLUDED

#include "../HumToonPredefine.hlsl"
#include "../../ShaderLibrary/Func.hlsl"

half HumCalcExShadeSmoothstep(half halfLambert, half shadeBorderPos, half shadeBorderBlur, half exShadeWidth)
{
    half startCenter = shadeBorderPos + exShadeWidth;
    half endCenter   = shadeBorderPos - exShadeWidth;

    // NOTE: blurはwidthを超えてはいけない(グラデが汚くなるため)
    shadeBorderBlur = shadeBorderBlur > exShadeWidth ? exShadeWidth : shadeBorderBlur;

    half start = HumBlurStep(startCenter, shadeBorderBlur, halfLambert);
    half end   = HumBlurStep(endCenter,   shadeBorderBlur, halfLambert);

    return halfLambert > shadeBorderPos ? OneMinus(start) : end;
}

half3 MixFirstShade(float2 uv, half3 baseColor, half halfLambert, half shadowAttenuation
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseColorWithoutBaseColor
#endif
)
{
#if defined(_HUM_USE_EX_FIRST_SHADE)
    // NOTE: Exを使用する場合は、FirstShadeの境界がExで上書きされるため、smoothstepをしないほうが綺麗になる
    half firstShade =  step(_FirstShadeBorderPos, halfLambert);
#else
    half firstShade = HumBlurStep(_FirstShadeBorderPos, _FirstShadeBorderBlur, halfLambert);
#endif
    firstShade *= shadowAttenuation;

    // First Shade Color
#if defined(_HUM_USE_FIRST_SHADE_MAP)
    half3 firstShadeMapColor = SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 firstShadeMapColor = baseColorWithoutBaseColor;
#endif
    half3 firstShadeColor = firstShadeMapColor * _FirstShadeColor.rgb;

    // Composite
    half3 finalFirstShadedColor = lerp(firstShadeColor, baseColor, firstShade);

#if defined(_HUM_USE_EX_FIRST_SHADE)
    // Ex
    half exFirstShade = HumCalcExShadeSmoothstep(halfLambert, _FirstShadeBorderPos, _FirstShadeBorderBlur, _ExFirstShadeWidth);
    half3 exFirstShadeColor = baseColorWithoutBaseColor * _ExFirstShadeColor;
    finalFirstShadedColor = lerp(finalFirstShadedColor, exFirstShadeColor, exFirstShade);
#endif

    return finalFirstShadedColor;
}

half3 MixSecondShade(float2 uv, half3 originalColor, half halfLambert
#if NOT(defined(_HUM_USE_SECOND_SHADE_MAP))
    , half3 baseColorWithoutBaseColor
#endif
)
{
    half secondShade = (HumBlurStep(_SecondShadeBorderPos, _SecondShadeBorderBlur, halfLambert));

    // Second Shade Color
#ifdef _HUM_USE_SECOND_SHADE_MAP
    half3 secondShadeMapColor = SAMPLE_TEXTURE2D(_SecondShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 secondShadeMapColor = baseColorWithoutBaseColor;
#endif
    half3 secondShadeColor = secondShadeMapColor * _SecondShadeColor.rgb;

    return lerp(secondShadeColor, originalColor, secondShade);
}

half3 MixPosAndBlurShade(float2 uv, half3 baseColor, half halfLambert, half shadowAttenuation
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseColorWithoutBaseColor
#endif
)
{
    half3 finalShadedColor = baseColor;

#if defined(_HUM_USE_FIRST_SHADE)
    finalShadedColor = MixFirstShade(uv, finalShadedColor, halfLambert, shadowAttenuation
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseColorWithoutBaseColor
    #endif
    );
#endif

#if defined(_HUM_USE_SECOND_SHADE)
    finalShadedColor = MixSecondShade(uv, finalShadedColor, halfLambert
    #if NOT(defined(_HUM_USE_SECOND_SHADE_MAP))
        , baseColorWithoutBaseColor
    #endif
    );
#endif

    return finalShadedColor;
}


half3 MixRampShade(float2 uv, half3 baseColor, half halfLambert)
{
    half3 finalShadedColor = baseColor;

#if defined(_HUM_USE_RAMP_SHADE)
    // NOTE: halfLambertの型はhalfではなくfloatのほうが良いかもしれない
    half anyValueIsAcceptable = 0;
    half2 rampUV = half2(halfLambert, anyValueIsAcceptable);
    half3 rampMapColor = _RampShadeMap.Sample(hum_sampler_linear_clamp, rampUV).rgb;
    finalShadedColor *= rampMapColor;
#endif

    return finalShadedColor;
}

half CalcShadeHalfLambert(float2 uv, float3 normalWS, float3 mainLightDirWS)
{
    half halfLambert = CalcHalfLambert(normalWS, mainLightDirWS);
#if defined(_HUM_USE_SHADE_CONTROL_MAP)
    half shadeControl = SAMPLE_TEXTURE2D(_ShadeControlMap, sampler_BaseMap, uv).r;
    shadeControl = lerp(ONE, shadeControl, _ShadeControlMapIntensity);
    halfLambert *= shadeControl;
#endif

    return halfLambert;
}

half3 HumMixShadeColor(float2 uv, half3 baseColor, float3 normalWS, float3 mainLightDirWS, half shadowAttenuation
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseColorWithoutBaseColor
#endif
)
{
    half halfLambert = CalcShadeHalfLambert(uv, normalWS, mainLightDirWS);

    half3 finalShadedColor = 0;

#if defined(_HUM_SHADE_MODE_POS_AND_BLUR)
    finalShadedColor = MixPosAndBlurShade(uv, baseColor, halfLambert, shadowAttenuation
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseColorWithoutBaseColor
    #endif
    );
#endif

#if defined(_HUM_SHADE_MODE_RAMP)
    finalShadedColor = MixRampShade(uv, baseColor, halfLambert);
#endif

    return finalShadedColor;
}

#endif
