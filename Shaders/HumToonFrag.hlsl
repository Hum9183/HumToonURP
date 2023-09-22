#ifndef HUM_TOON_FRAG_INCLUDED
#define HUM_TOON_FRAG_INCLUDED

#include "HumToonFragIncludes.hlsl"

void LitPassFragment(
    Varyings input
    , out half4 outColor : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv0 = input.uv;

    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(uv0, surfaceData);

#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    // TODO: SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);
    // TODO: Decal
    // TODO: CanDebugOverrideOutputColor()
    // TODO: _LIGHT_LAYERS
    // TODO: Forward plus
    // TODO: SSAOのweight調整機能
    // TODO: normalのoverride(顔の法線を正面に向ける等)
    // TODO: 関数名にHumをつける(被り対策)

    // Main light
    half4 shadowMask = CalculateShadowMask(inputData);
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

    // Frag
    half4 finalColor;
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    half3 baseMapColor;
    finalColor = CalcBaseColor(uv0, baseMapColor);
#else
    finalColor = CalcBaseColor(uv0);
#endif

#if defined(_HUM_USE_FIRST_SHADE) || defined(_HUM_USE_SECOND_SHADE)
    finalColor.rgb = MixShade(uv0, finalColor.rgb, inputData.normalWS, mainLight.direction
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseMapColor
    #endif
    );
#endif

    // Get light colors
    half3 mainLightColor = CalcMainLightColor(mainLight.color.rgb);

    // Get others
#if defined(_HUM_USE_RIM_LIGHT)
    half3 rimLightColor = HumCalcRimLightColor(uv0, inputData.normalWS, inputData.viewDirectionWS, mainLightColor);
#endif

#if defined(_HUM_USE_EMISSION)
    half3 emissionColor = HumCalcEmissionColor(uv0, mainLightColor);
#endif

#if defined(_HUM_USE_MAT_CAP)
    half3 matCapColor = HumCalcMatCapColor(inputData.normalWS, inputData.viewDirectionWS, mainLightColor);
#endif

#if defined(_ADDITIONAL_LIGHTS)
    half3 additionalLightsColor = CalcAdditionalLightColor(finalColor.rgb, inputData, shadowMask, aoFactor);
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
    half3 additionalLightsColorVertex = CalcAdditionalLightColorVertex(finalColor.rgb, inputData.vertexLighting);
#endif

    // Final composite
    finalColor.rgb = MixMainLightColor(finalColor.rgb, mainLightColor);
#if defined(_HUM_USE_RIM_LIGHT)
    finalColor.rgb += rimLightColor;
#endif

#if defined(_HUM_USE_EMISSION)
    finalColor.rgb = emissionColor;
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

#ifdef _WRITE_RENDERING_LAYERS
    SetRenderingLayers(outRenderingLayers);
#endif
}

#endif
