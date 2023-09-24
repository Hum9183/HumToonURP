#ifndef HUM_TOON_FRAG_INCLUDED
#define HUM_TOON_FRAG_INCLUDED

#include "HumToonFragIncludes.hlsl"

void frag(
    Varyings input
    , out half4 outColor : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    // ************************************ //
    // ****** Calculate what PS needs ***** //
    // ************************************ //

    // UV
    float2 uv0 = input.uv;

    // SurfaceData
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(uv0, surfaceData);

    // LOD Fade
#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    // InputData
    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    // TODO: SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);
    // TODO: Decal
    // TODO: CanDebugOverrideOutputColor()
    // TODO: SSAOのweight調整機能
    // TODO: normalのoverride(顔の法線を正面に向ける等)
    // TODO: 関数名にHumをつける(被り対策)

    // Shadow
    half4 shadowMask = CalculateShadowMask(inputData);
    // AO
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    // Main light
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

    // Mesh Rendering Layers
#if defined(_LIGHT_LAYERS)
    uint meshRenderingLayers = GetMeshRenderingLayer();
#endif

    // Base
    half4 baseColor;
    half3 baseMapColor;
    HumCalcBaseColor(uv0, baseColor, baseMapColor);

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
    half3 matCapColor = HumCalcMatCapColor(inputData.normalWS, inputData.viewDirectionWS, mainLightColor);
#endif

#if defined(_ADDITIONAL_LIGHTS)
    half3 additionalLightsColor = CalcAdditionalLightColor(uv0, baseColor.rgb, inputData, shadowMask, aoFactor
    #if defined(_LIGHT_LAYERS)
        , meshRenderingLayers
    #endif
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseMapColor
    #endif
    );
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    half3 additionalLightsColorVertex = CalcAdditionalLightColorVertex(baseColor.rgb, inputData.vertexLighting);
#endif


    // ********************** //
    // ****** Composite ***** //
    // ********************** //
    half4 finalColor = 0;

#if defined(_HUM_USE_FIRST_SHADE) || defined(_HUM_USE_SECOND_SHADE) || defined(_HUM_USE_RAMP_SHADE)
    // Mix Base Color and Shade Color
    finalColor.rgb = HumMixShadeColor(uv0, baseColor, inputData.normalWS, mainLight.direction
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseMapColor
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
    finalColor.rgb += emissionColor;
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

    finalColor.rgb = MixFog(finalColor.rgb, inputData.fogCoord);
    finalColor.a = OutputAlpha(finalColor.a, IsSurfaceTypeTransparent(_SurfaceType));

    outColor = finalColor;

    // Rendering Layers
#ifdef _WRITE_RENDERING_LAYERS
    SetRenderingLayers(outRenderingLayers);
#endif
}

#endif
