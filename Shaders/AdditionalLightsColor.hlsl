#ifndef ADDITIONAL_LIGHTS_COLOR_INCLUDED
#define ADDITIONAL_LIGHTS_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

half3 CalcAdditionalLightColor(
    half3 originalColor, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor)
{
    uint pixelLightCount = GetAdditionalLightsCount();
    half3 additionalLightsColor;

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
        // TODO: _LIGHT_LAYERS
        // TODO: Specular
        half NdotL = saturate(dot(inputData.normalWS, light.direction));
        half3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL;
        additionalLightsColor += originalColor * lightColor;
    LIGHT_LOOP_END

    return additionalLightsColor * _AdditionalLightsColorWeight;
}

half3 CalcAdditionalLightColorVertex(half3 originalColor, half3 vertexLighting)
{
    return originalColor * vertexLighting * _AdditionalLightsColorWeight;
}

#endif
