#ifndef HT_CALC_COLOR_INCLUDED
#define HT_CALC_COLOR_INCLUDED

#include "HTFragIncludes.hlsl"

half4 HTCalcFragColor(float2 uv0, InputData inputData, SurfaceData surfaceData)
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
#if defined(_LIGHT_COOKIES) && defined(_HT_USE_MAIN_LIGHT_COOKIE_AS_SHADE)
    Light mainLight = HTGetMainLight(inputData, shadowMask, aoFactor);
#else
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);
#endif

    // Shadow attenuation
    half shadowAttenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
#if defined(_LIGHT_COOKIES) && defined(_HT_USE_MAIN_LIGHT_COOKIE_AS_SHADE)
    real3 cookieColor = SampleMainLightCookie(inputData.positionWS);
    shadowAttenuation *= cookieColor.r; // NOTE: RGB Format is not supported.
#endif

    // Global illumination
#if defined(_HT_RECEIVE_GI)
    half3 giColor = HTGlobalIllumination(
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
    HTCalcBaseColor(uv0, surfaceData, baseColor, baseMapColorOnly);

    // Main Light Diffuse
    half3 mainLightDiffuse = HTCalcMainLightDiffuse(
        mainLight
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    );

    // Main Light Specular
    half3 mainLightSpecular = HTMainLightSpecular(
        brdfData, mainLight, inputData.normalWS, inputData.viewDirectionWS
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    );

    // Rim Light
#if defined(_HT_USE_RIM_LIGHT)
    half3 rimLightColor = HTCalcRimLightColor(uv0, inputData.normalWS, inputData.viewDirectionWS, mainLightDiffuse);
#endif

    // Emission
#if defined(_HT_USE_EMISSION)
    half3 emissionColor = HTCalcEmissionColor(uv0, baseColor.rgb);
#endif

    // Mat Cap
#if defined(_HT_USE_MAT_CAP)
    half3 matCapColor = HTCalcMatCapColor(inputData.normalWS, inputData.viewDirectionWS, mainLightDiffuse
    #if defined(_HT_USE_MAT_CAP_MASK)
        , uv0
    #endif
    );
#endif

    // Additional Lights
#if defined(_ADDITIONAL_LIGHTS)
    half3 additionalLightsColor = HTCalcAdditionalLights(uv0, baseColor.rgb, inputData, shadowMask, aoFactor
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    #if defined(_HT_USE_ADDITIONAL_LIGHTS_SPECULAR)
        , brdfData
        , inputData.viewDirectionWS
    #endif
    );
#endif

    // Additional Lights Vertex
#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    half3 additionalLightsColorVertex = HTCalcAdditionalLightColorVertex(baseColor.rgb, inputData.vertexLighting);
#endif


    // ********************** //
    // ****** Composite ***** //
    // ********************** //

    // *** Toon Color ***
    half3 toonColor = 0;

#if defined(_HT_USE_FIRST_SHADE) || defined(_HT_USE_SECOND_SHADE) || defined(_HT_USE_RAMP_SHADE)
    // Mix Base Color and Shade Color
    toonColor = HTMixShadeColor(uv0, baseColor, inputData.normalWS, mainLight.direction, shadowAttenuation
    #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    );
#else
    // Base Color
    toonColor = baseColor;
#endif

#if defined(_HT_OVERRIDE_EMISSION_COLOR)
    toonColor = HTOverrideEmissionColor(toonColor, emissionColor);
#endif

    // Mix Main Light
    toonColor = HTMixMainLight(toonColor, mainLightDiffuse, mainLightSpecular);

#if defined(_HT_USE_RIM_LIGHT)
    toonColor += rimLightColor;
#endif

#if defined(_HT_USE_MAT_CAP)
    toonColor += matCapColor;
#endif


    // *** LightingData ***
    LightingData lightingData = (LightingData)0;

    lightingData.mainLightColor = toonColor;

#if defined(_HT_RECEIVE_GI)
    lightingData.giColor += giColor;
#endif

#if defined(_ADDITIONAL_LIGHTS)
    lightingData.additionalLightsColor += additionalLightsColor;
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    lightingData.vertexLightingColor += additionalLightsColorVertex;
#endif

#if defined(_HT_USE_EMISSION)
    lightingData.emissionColor += HTCalcEmissionColorIntensity(emissionColor);
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
