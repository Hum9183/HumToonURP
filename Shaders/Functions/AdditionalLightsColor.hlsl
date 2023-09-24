#ifndef ADDITIONAL_LIGHTS_COLOR_INCLUDED
#define ADDITIONAL_LIGHTS_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "../../ShaderLibrary/Func.hlsl"
#include "HumToonShade.hlsl"

half3 CalcAdditionalLightColorInternal(half3 originalColor, float3 normalWS, Light light)
{
    // TODO: Specular
    half NdotL = saturate(dot(normalWS, light.direction));
    half3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL;
    return originalColor * lightColor;
}

// EXPERIMENTAL: AdditionalLightのDirectionalLightはShadeと同じ計算法を使用する
half3 HumCalcAdditionalDirectionalLight(float2 uv, half3 baseColor, float3 normalWS, Light additionalDirectionalLight
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseMapColor
#endif
)
{
    // TODO: Textureなどはサンプルし直しになるため負荷がかかる。
    // structなどに保持することを検討する。
    half shadowAttenuation = additionalDirectionalLight.distanceAttenuation * additionalDirectionalLight.shadowAttenuation;
    half3 shadedColor = HumMixShadeColor(uv, baseColor, normalWS, additionalDirectionalLight.direction, shadowAttenuation
    #if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
        , baseMapColor
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

half3 CalcAdditionalLightColor(
    float2 uv, half3 baseColor, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    , half3 baseMapColor
#endif
)
{
    // NOTE:
    // 2灯目以降のDirectionalLightはPointLight扱いとなるため、
    // Litと比べルックの差異が大きい。
    // 1灯目のDirectionalLightをShadeで計算している都合、回避できない。

    // FUTURE:
    // PointLightの計算もShadeと同様の計算を行うアプローチがある。
    // できればPointLightはLitと同じ計算法にしたい。
    // PointLight扱いのDirectionalLightの判定方法があれば、明確にDirectionalLightの計算を分けることが出来そう。

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
                additionalLightsColor += CalcAdditionalLightColorInternal(baseColor, normalWS, light);
            }
        }
    LIGHT_LOOP_END

    return additionalLightsColor * _AdditionalLightsColorWeight;
}

half3 CalcAdditionalLightColorVertex(half3 originalColor, half3 vertexLighting)
{
    return originalColor * vertexLighting * _AdditionalLightsColorWeight;
}

#endif
