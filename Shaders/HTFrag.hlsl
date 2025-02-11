#ifndef HT_FRAG_INCLUDED
#define HT_FRAG_INCLUDED

#include "HTCalcFragColor.hlsl"

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

    // UV
    const float2 uv0 = input.uv;

    // SurfaceData
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(uv0, surfaceData);

    // LOD Fade
#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    // InputData
    InputData inputData;
    HTInitializeInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, uv0, _BaseMap);

    // TODO:
    // - SSAOのweight調整機能
    // - normalのoverride(顔の法線を正面に向ける等)
    // - Varyings SurfaceData InputData BRDFDataの整理

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    half4 finalColor = 0;
    finalColor = HTCalcFragColor(uv0, inputData, surfaceData);
    finalColor.rgb = MixFog(finalColor.rgb, inputData.fogCoord);
    finalColor.a = OutputAlpha(finalColor.a, IsSurfaceTypeTransparent(_SurfaceType));

    outColor = finalColor;

    // Rendering Layers
#ifdef _WRITE_RENDERING_LAYERS
    SetRenderingLayers(outRenderingLayers);
#endif
}

#endif
