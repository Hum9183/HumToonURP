#ifndef ADDITIONAL_LIGHTS_COLOR_INCLUDED
#define ADDITIONAL_LIGHTS_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

half3 CalcAdditionalLightColor(
    half3 originalColor, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
)
{
    // NOTE:
    // 2灯目以降のDirectionalLightはPointLight扱いとなるため、
    // Litと比べルックの差異が大きい。
    // 1灯目のDirectionalLightをShadeで計算している都合、回避できない。

    // FEATURE:
    // PointLightの計算もShadeと同様の計算を行うアプローチがある。
    // できればPointLightはLitと同じ計算法にしたい。
    // PointLight扱いのDirectionalLightの判定方法があれば、明確にDirectionalLightの計算を分けることが出来そう。

    uint pixelLightCount = GetAdditionalLightsCount();
    half3 additionalLightsColor;

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
    #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
    #endif
        {
            // TODO: Specular
            half NdotL = saturate(dot(inputData.normalWS, light.direction));
            half3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL;
            additionalLightsColor += originalColor * lightColor;
        }
    LIGHT_LOOP_END

    return additionalLightsColor * _AdditionalLightsColorWeight;
}

half3 CalcAdditionalLightColorVertex(half3 originalColor, half3 vertexLighting)
{
    return originalColor * vertexLighting * _AdditionalLightsColorWeight;
}

#endif
