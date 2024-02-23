#ifndef HUM_TOON_ADDITIONAL_LIGHTS_COLOR_INCLUDED
#define HUM_TOON_ADDITIONAL_LIGHTS_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "../../ShaderLibrary/Func.hlsl"
#include "HumToonShade.hlsl"

half3 HumCalcAdditionalLightColorInternal(half3 originalColor, float3 normalWS, Light light)
{
    // TODO: Specular
    half NdotL = saturate(dot(normalWS, light.direction));
    half3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL;
    return originalColor * lightColor;
}

// EXPERIMENTAL: AdditionalLightのDirectionalLightはShadeと同じ計算法を使用する
half3 HumCalcAdditionalDirectionalLight(float2 uv, half3 baseColor, float3 normalWS, Light additionalDirectionalLight
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseColorWithoutBaseColor
#endif
)
{
    // TODO: Textureなどはサンプルし直しになるため負荷がかかる。
    // structなどに保持することを検討する。
    half shadowAttenuation = additionalDirectionalLight.distanceAttenuation * additionalDirectionalLight.shadowAttenuation;
    half3 shadedColor = HumMixShadeColor(uv, baseColor, normalWS, additionalDirectionalLight.direction, shadowAttenuation
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseColorWithoutBaseColor
    #endif
    );

    return shadedColor * additionalDirectionalLight.color;
}

// NOTE: AdditionalLight用
bool HumIsDirectionalLight(uint lightIndex)
{
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
    float4 lightPositionWS = _AdditionalLightsBuffer[lightIndex].position;
#else
    float4 lightPositionWS = _AdditionalLightsPosition[lightIndex];
#endif
    return lightPositionWS.w == 0.0 ? true : false;
}

half3 HumCalcAdditionalLightColor(
    float2 uv, half3 baseColor, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseColorWithoutBaseColor
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
            additionalLightsColor += HumCalcAdditionalDirectionalLight(uv, baseColor, normalWS, light
            #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
                , baseColor
            #endif
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
            if (HumIsDirectionalLight(lightIndex))
            {
                additionalLightsColor += HumCalcAdditionalDirectionalLight(uv, baseColor, normalWS, light
                #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
                    , baseColor
                #endif
                );
            }
            else
            {
                additionalLightsColor += HumCalcAdditionalLightColorInternal(baseColor, normalWS, light);
            }
        }
    LIGHT_LOOP_END

    return additionalLightsColor * _AdditionalLightsColorWeight;
}

half3 HumCalcAdditionalLightColorVertex(half3 originalColor, half3 vertexLighting)
{
    return originalColor * vertexLighting * _AdditionalLightsColorWeight;
}

#endif
