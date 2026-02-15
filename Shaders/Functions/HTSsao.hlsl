#ifndef HT_SSAO
#define HT_SSAO

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "..\..\ShaderLibrary\HTUtils.hlsl"

#define DEFAULT_SSAO (1)

// NOTE: テクスチャからAOを乗算できる機能を提供しても良いかもしれない
AmbientOcclusionFactor HTGetSsao(float2 uv, InputData inputData, SurfaceData surfaceData)
{
    AmbientOcclusionFactor aoFactor = (AmbientOcclusionFactor)DEFAULT_SSAO;
#if defined(_HT_RECEIVE_SSAO)
    aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);

    half ssaoIntensity = _SsaoIntensity;

#if defined(_HT_USE_SSAO_MASK)
    // Sample SSAO mask and reduce intensity in masked areas
    // Black (0) = disable SSAO, White (1) = enable SSAO
    half ssaoMask = SAMPLE_TEXTURE2D(_SsaoMask, sampler_BaseMap, uv).r;
    ssaoIntensity *= lerp(DEFAULT_SSAO, ssaoMask, _SsaoMaskIntensity);
#endif

    aoFactor.indirectAmbientOcclusion = lerp(DEFAULT_SSAO, aoFactor.indirectAmbientOcclusion, ssaoIntensity);
    aoFactor.directAmbientOcclusion = lerp(DEFAULT_SSAO, aoFactor.directAmbientOcclusion, ssaoIntensity);
#endif
    return aoFactor;
}

#endif
