#ifndef HT_ADDITIONAL_LIGHTS_INCLUDED
#define HT_ADDITIONAL_LIGHTS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "..\..\ShaderLibrary\HTUtils.hlsl"
#include "HTShade.hlsl"

half3 HTCalcAdditionalLightInternal(
    Light light, half3 baseColor, float3 normalWS
#if defined(_HT_USE_ADDITIONAL_LIGHTS_SPECULAR)
    , BRDFData brdfData
    , half3 viewDirectionWS
#endif
)
{
    half3 additionalLightColor = baseColor;
    half NdotL = saturate(dot(normalWS, light.direction));
    half3 radiance = light.color * light.distanceAttenuation * NdotL;

#if defined(_HT_USE_ADDITIONAL_LIGHTS_SPECULAR)
    half3 addtionalLightSpecular = DirectBRDFSpecular(brdfData, normalWS, light.direction, viewDirectionWS);
    additionalLightColor += addtionalLightSpecular * brdfData.specular * _AdditionalLightsSpecularIntensity;
#endif

    return additionalLightColor * radiance;
}

// EXPERIMENTAL: AdditionalLightのDirectionalLightはShadeと同じ計算法を使用する
// TODO: Specular
half3 HTCalcAdditionalDirectionalLight(float2 uv, half3 baseColor, float3 normalWS, Light additionalDirectionalLight
#ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
    , half3 baseMapColorOnly
#endif
)
{
    // TODO: Textureなどはサンプルし直しになるため負荷がかかる。
    // structなどに保持することを検討する。
    half shadowAttenuation = additionalDirectionalLight.distanceAttenuation * additionalDirectionalLight.shadowAttenuation;
    half3 shadedColor = HTMixShadeColor(uv, baseColor, normalWS, additionalDirectionalLight.direction, shadowAttenuation
    #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
        , baseMapColorOnly
    #endif
    );

    return shadedColor * additionalDirectionalLight.color;
}

// NOTE: AdditionalLight用
bool HTIsDirectionalLight(uint lightIndex)
{
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
    float4 lightPositionWS = _AdditionalLightsBuffer[lightIndex].position;
#else
    float4 lightPositionWS = _AdditionalLightsPosition[lightIndex];
#endif
    return lightPositionWS.w == 0.0 ? true : false;
}

half3 HTCalcAdditionalLights(
    float2 uv, half3 baseColor, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
#ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
    , half3 baseMapColorOnly
#endif
#if defined(_HT_USE_ADDITIONAL_LIGHTS_SPECULAR)
    , BRDFData brdfData
    , half3 viewDirectionWS
#endif
)
{
    // BUG: per-vertex light layers not working
    // https://github.com/Unity-Technologies/Graphics/commit/2dcf83236d89dbe29dc3b1d0fe5a1f1658303842

    float3 normalWS = inputData.normalWS;
    uint pixelLightCount = GetAdditionalLightsCount();
    half3 additionalLightsColor;

#if USE_FORWARD_PLUS
    // NOTE: ForwardPlus時の2灯目以降のDirectionalLightの計算
    // (無印Forwardでは2灯目以降のDirectionalLightはPointLightと一緒に計算されるが、
    // ForwardPlusでは個別で処理を書く必要がある)
    for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
    #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
    #endif
        {
            additionalLightsColor += HTCalcAdditionalDirectionalLight(uv, baseColor, normalWS, light
            #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
                , baseMapColorOnly
            #endif
            // TODO: Specular
            );
        }
    }
#endif

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
    #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
    #endif
        {
            // TODO: おそらく重いため、もう少し最適化する
            if (HTIsDirectionalLight(lightIndex))
            {
                additionalLightsColor += HTCalcAdditionalDirectionalLight(uv, baseColor, normalWS, light
                #ifdef _HT_REQUIRES_BASE_MAP_COLOR_ONLY
                    , baseColor
                #endif
                );
            }
            else
            {
                additionalLightsColor += HTCalcAdditionalLightInternal(
                    light, baseColor, normalWS
                    #if defined(_HT_USE_ADDITIONAL_LIGHTS_SPECULAR)
                        , brdfData
                        , viewDirectionWS
                    #endif
                );
            }
        }
    LIGHT_LOOP_END

    return additionalLightsColor * _AdditionalLightsColorWeight;
}

half3 HTCalcAdditionalLightColorVertex(half3 originalColor, half3 vertexLighting)
{
    return originalColor * vertexLighting * _AdditionalLightsColorWeight;
}

#endif
