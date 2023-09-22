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

half3 MixFirstShade(float2 uv, half3 originalColor, half halfLambert
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseMapColor
#endif
)
{
#if defined(_HUM_USE_EX_FIRST_SHADE)
    // NOTE: Exを使用する場合は、FirstShadeの境界がExで上書きされるため、smoothstepをしないほうが綺麗になる
    half firstShade =  OneMinus(step(_FirstShadeBorderPos, halfLambert));
#else
    half firstShade = OneMinus(HumBlurStep(_FirstShadeBorderPos, _FirstShadeBorderBlur, halfLambert));
#endif

#if defined(_HUM_USE_FIRST_SHADE_MAP)
    half3 firstShadeMapColor = SAMPLE_TEXTURE2D(_FirstShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 firstShadeMapColor = baseMapColor;
#endif
    half3 firstShadeColor = firstShadeMapColor * _FirstShadeColor.rgb;

    half3 finalFirstShadedColor;
    finalFirstShadedColor = lerp(originalColor, firstShadeColor, firstShade);
#if defined(_HUM_USE_EX_FIRST_SHADE)
    half exFirstShade = HumCalcExShadeSmoothstep(halfLambert, _FirstShadeBorderPos, _FirstShadeBorderBlur, _ExFirstShadeWidth);
    half3 exFirstShadeColor = baseMapColor * _ExFirstShadeColor;
    finalFirstShadedColor = lerp(finalFirstShadedColor, exFirstShadeColor, exFirstShade);
#endif

    return finalFirstShadedColor;
}

half3 MixSecondShade(float2 uv, half3 originalColor, half halfLambert
#if NOT(defined(_HUM_USE_SECOND_SHADE_MAP))
    , half3 baseMapColor
#endif
)
{
    half secondShade = OneMinus(HumBlurStep(_SecondShadeBorderPos, _SecondShadeBorderBlur, halfLambert));

#ifdef _HUM_USE_SECOND_SHADE_MAP
    half3 secondShadeMapColor = SAMPLE_TEXTURE2D(_SecondShadeMap, sampler_BaseMap, uv).rgb;
#else
    half3 secondShadeMapColor = baseMapColor;
#endif
    half3 secondShadeColor = secondShadeMapColor * _SecondShadeColor.rgb;

    return lerp(originalColor, secondShadeColor, secondShade);
}

half3 MixShade(float2 uv, half3 originalColor, float3 normalWS, float3 mainLightDirWS
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseMapColor
#endif
)
{
    half3 finalShadedColor = originalColor;

    half halfLambert = CalcHalfLambert(normalWS, mainLightDirWS);

#if defined(_HUM_USE_FIRST_SHADE)
    finalShadedColor = MixFirstShade(uv, finalShadedColor, halfLambert
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseMapColor
    #endif
    );
#endif

#if defined(_HUM_USE_SECOND_SHADE)
    finalShadedColor = MixSecondShade(uv, finalShadedColor, halfLambert
    #if NOT(defined(_HUM_USE_SECOND_SHADE_MAP))
        , baseMapColor
    #endif
    );
#endif

    return finalShadedColor;
}

#endif
