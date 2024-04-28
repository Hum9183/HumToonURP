#ifndef CALC_HUM_TOON_COLOR_INCLUDED
#define CALC_HUM_TOON_COLOR_INCLUDED

#include "HumToonFragIncludes.hlsl"

half4 CalcHumToonColor(float2 uv0, InputData inputData, SurfaceData surfaceData)
{
    // BRDF
    BRDFData brdfData;
    InitializeBRDFData(surfaceData, brdfData); // NOTE: can modify "surfaceData"...

    // Debug Display
#if defined(DEBUG_DISPLAY)
    half4 debugColor;
    if (CanDebugOverrideOutputColor(inputData, surfaceData, brdfData, debugColor))
    {
        return debugColor;
    }
#endif

    // Shadow
    half4 shadowMask = CalculateShadowMask(inputData);
    // AO
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    // Main light
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);
    half shadowAttenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;

    // Global illumination
    half3 giColor = HumGlobalIllumination(
        brdfData, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,
        inputData.normalWS, inputData.viewDirectionWS, inputData.normalizedScreenSpaceUV);

    // Mesh Rendering Layers
#if defined(_LIGHT_LAYERS)
    uint meshRenderingLayers = GetMeshRenderingLayer();
#endif

    // Base
    half4 baseColor;
    half3 baseColorWithoutBaseColor; // NOTE: _BaseColorが適用されていないBaseColor
    HumCalcBaseColor(uv0, surfaceData, baseColor, baseColorWithoutBaseColor);

    // Main Light Color
    half3 mainLightColor = HumCalcMainLightColor(
        mainLight
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    );

    // Get others
#if defined(_HUM_USE_RIM_LIGHT)
    half3 rimLightColor = HumCalcRimLightColor(uv0, inputData.normalWS, inputData.viewDirectionWS, mainLightColor);
#endif

#if defined(_HUM_USE_EMISSION)
    half3 emissionColor = HumCalcEmissionColor(uv0, baseColor.rgb);
#endif

#if defined(_HUM_USE_MAT_CAP)
    half3 matCapColor = HumCalcMatCapColor(inputData.normalWS, inputData.viewDirectionWS, mainLightColor
    #if defined(_HUM_USE_MAT_CAP_MASK)
        , uv0
    #endif
    );
#endif

#if defined(_ADDITIONAL_LIGHTS)
    half3 additionalLightsColor = HumCalcAdditionalLightColor(uv0, baseColor.rgb, inputData, shadowMask, aoFactor
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseColorWithoutBaseColor
    #endif
    );
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    half3 additionalLightsColorVertex = HumCalcAdditionalLightColorVertex(baseColor.rgb, inputData.vertexLighting);
#endif


    // ********************** //
    // ****** Composite ***** //
    // ********************** //
    half4 finalColor = 0;

#if defined(_HUM_USE_FIRST_SHADE) || defined(_HUM_USE_SECOND_SHADE) || defined(_HUM_USE_RAMP_SHADE)
    // Mix Base Color and Shade Color
    finalColor.rgb = HumMixShadeColor(uv0, baseColor, inputData.normalWS, mainLight.direction, shadowAttenuation
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseColorWithoutBaseColor
    #endif
    );
#else
    // Base Color
    finalColor.rgb = baseColor;
#endif

#if defined(_HUM_OVERRIDE_EMISSION_COLOR)
    finalColor.rgb = HumOverrideEmissionColor(finalColor.rgb, emissionColor);
#endif

    // Mix Main Light Color
    finalColor.rgb = MixMainLightColor(finalColor.rgb, mainLightColor);

#if defined(_HUM_USE_RIM_LIGHT)
    finalColor.rgb += rimLightColor;
#endif

#if defined(_HUM_USE_EMISSION)
    finalColor.rgb += HumCalcEmissionColorIntensity(emissionColor);
#endif

#if defined(_HUM_USE_MAT_CAP)
    finalColor.rgb += matCapColor;
#endif

#if defined(_ADDITIONAL_LIGHTS)
    finalColor.rgb += additionalLightsColor;
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    finalColor.rgb += additionalLightsColorVertex;
#endif

    finalColor.rgb += giColor;

    return finalColor;
}

#endif
