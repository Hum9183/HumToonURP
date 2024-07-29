#ifndef CALC_HUM_TOON_COLOR_INCLUDED
#define CALC_HUM_TOON_COLOR_INCLUDED

#include "HumToonFragIncludes.hlsl"

half4 CalcHumToonFragColor(float2 uv0, InputData inputData, SurfaceData surfaceData)
{
    // ************************************ //
    // ****** Calculate what PS needs ***** //
    // ************************************ //

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
#if defined(_HUM_RECEIVE_GI)
    half3 giColor = HumGlobalIllumination(
        brdfData, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,
        inputData.normalWS, inputData.viewDirectionWS, inputData.normalizedScreenSpaceUV);
#endif

    // Mesh Rendering Layers
#if defined(_LIGHT_LAYERS)
    uint meshRenderingLayers = GetMeshRenderingLayer();
#endif

    // Base
    half4 baseColor;
    half3 baseMapColorOnly;
    HumCalcBaseColor(uv0, surfaceData, baseColor, baseMapColorOnly);

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
    #ifdef _HUM_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    );
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    half3 additionalLightsColorVertex = HumCalcAdditionalLightColorVertex(baseColor.rgb, inputData.vertexLighting);
#endif


    // ********************** //
    // ****** Composite ***** //
    // ********************** //

    // *** Toon Color ***
    half3 toonColor = 0;

#if defined(_HUM_USE_FIRST_SHADE) || defined(_HUM_USE_SECOND_SHADE) || defined(_HUM_USE_RAMP_SHADE)
    // Mix Base Color and Shade Color
    toonColor = HumMixShadeColor(uv0, baseColor, inputData.normalWS, mainLight.direction, shadowAttenuation
    #ifdef _HUM_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    );
#else
    // Base Color
    toonColor = baseColor;
#endif

#if defined(_HUM_OVERRIDE_EMISSION_COLOR)
    toonColor = HumOverrideEmissionColor(toonColor, emissionColor);
#endif

    // Mix Main Light Color
    toonColor = MixMainLightColor(toonColor, mainLightColor);

#if defined(_HUM_USE_RIM_LIGHT)
    toonColor += rimLightColor;
#endif

#if defined(_HUM_USE_MAT_CAP)
    toonColor += matCapColor;
#endif


    // *** LightingData ***
    LightingData lightingData = (LightingData)0;

    lightingData.mainLightColor = toonColor;

#if defined(_HUM_RECEIVE_GI)
    lightingData.giColor += giColor;
#endif

#if defined(_ADDITIONAL_LIGHTS)
    lightingData.additionalLightsColor += additionalLightsColor;
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    lightingData.vertexLightingColor += additionalLightsColorVertex;
#endif

#if defined(_HUM_USE_EMISSION)
    lightingData.emissionColor += HumCalcEmissionColorIntensity(emissionColor);
#endif

    // *** Calculate Final Color ***
#if REAL_IS_HALF
    // Clamp any half.inf+ to HALF_MAX
    return min(CalculateFinalColor(lightingData, surfaceData.alpha), HALF_MAX);
#else
    return CalculateFinalColor(lightingData, surfaceData.alpha);
#endif
}

#endif
