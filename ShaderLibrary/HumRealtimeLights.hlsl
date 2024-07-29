#ifndef HUM_UNIVERSAL_REALTIME_LIGHTS_INCLUDED
#define HUM_UNIVERSAL_REALTIME_LIGHTS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookie.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Clustering.hlsl"

// NOTE: LightCookiesのifdefを取り除いた版
Light HumGetMainLight(float4 shadowCoord, float3 positionWS, half4 shadowMask)
{
    Light light = GetMainLight();
    light.shadowAttenuation = MainLightShadow(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);
    return light;
}

// NOTE: LightCookiesのifdefを取り除いた版
Light HumGetMainLight(InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor)
{
    Light light = HumGetMainLight(inputData.shadowCoord, inputData.positionWS, shadowMask);

#if defined(_SCREEN_SPACE_OCCLUSION) && !defined(_SURFACE_TYPE_TRANSPARENT)
    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_AMBIENT_OCCLUSION))
    {
        light.color *= aoFactor.directAmbientOcclusion;
    }
#endif

    return light;
}

#endif
