#ifndef HUM_TOON_FRAG_INCLUDED
#define HUM_TOON_FRAG_INCLUDED

#include "HumToonBaseColor.hlsl"
#include "HumToonShade.hlsl"
#include "../ShaderLibrary/RenderingLayers.hlsl"

#include "HumToonInput.hlsl"
#include "HumToonVaryings.hlsl"
#include "HumToonFunc.hlsl"
#include "InitializeInputData.hlsl"

#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

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

    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    // TODO: SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);
    // TODO: Decal
    // TODO: CanDebugOverrideOutputColor()

    half4 finalColor;
    finalColor     = CalcBaseColor(input.uv);
    finalColor.rgb = CalcShade(input.uv, finalColor.rgb, input.normalWS, _MainLightPosition.xyz);

    finalColor.rgb = MixFog(finalColor.rgb, inputData.fogCoord);
    finalColor.a = OutputAlpha(finalColor.a, IsSurfaceTypeTransparent(_SurfaceType));

    outColor = finalColor;

#ifdef _WRITE_RENDERING_LAYERS
    SetRenderingLayers(outRenderingLayers);
#endif
}

#endif
