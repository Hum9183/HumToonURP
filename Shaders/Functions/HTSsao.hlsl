#ifndef HT_SSAO
#define HT_SSAO

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "..\..\ShaderLibrary\HTUtils.hlsl"

#define DEFAULT_SSAO (1)

// NOTE: テクスチャからAOを乗算できる機能を提供しても良いかもしれない
AmbientOcclusionFactor HTGetSsao(InputData inputData, SurfaceData surfaceData)
{
    AmbientOcclusionFactor aoFactor = (AmbientOcclusionFactor)DEFAULT_SSAO;
#if defined(_HT_USE_SSAO)
    aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    aoFactor.indirectAmbientOcclusion = lerp(DEFAULT_SSAO, aoFactor.indirectAmbientOcclusion, _SsaoWeight);
    aoFactor.directAmbientOcclusion = lerp(DEFAULT_SSAO, aoFactor.directAmbientOcclusion, _SsaoWeight);
#endif
    return aoFactor;
}

#endif
