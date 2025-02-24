#ifndef HT_MAIN_LIGHT_INCLUDED
#define HT_MAIN_LIGHT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"

half3 HTCalcMainLightDiffuse(
    Light mainLight
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
)
{
    half3 mainLightDiffuse = 0;

#if defined(_LIGHT_LAYERS)
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
#endif
    {
        mainLightDiffuse = mainLight.color;
        // TODO: Limitは最終出力のリミットとして実装し直す
        // mainLightDiffuse = lerp(mainLightDiffuse, min(mainLightDiffuse, _MainLightDiffuseUpperLimit), _UseMainLightDiffuseUpperLimit);
        // mainLightDiffuse = lerp(mainLightDiffuse, max(mainLightDiffuse, _MainLightDiffuseLowerLimit), _UseMainLightDiffuseLowerLimit);
    }

    return mainLightDiffuse * _MainLightDiffuseIntensity;
}

half3 HTCalcMainLightSpecular(
    BRDFData brdfData, Light mainLight, half3 normalWS, half3 viewDirectionWS
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
)
{
    half3 mainLightSpecular = 0;

#if defined(_LIGHT_LAYERS)
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
#endif
    {
        half NdotL = saturate(dot(normalWS, mainLight.direction));
        mainLightSpecular = DirectBRDFSpecular(brdfData, normalWS, mainLight.direction, viewDirectionWS);
        mainLightSpecular *= NdotL * brdfData.specular * mainLight.color * mainLight.shadowAttenuation * _MainLightSpecularIntensity;
    }

    return mainLightSpecular * _MainLightSpecularIntensity;
}

half3 HTMixMainLight(half3 originalColor, half3 mainLightColor, half3 mainLightSpecular)
{
    // return lerp(
    //     originalColor,
    //     originalColor * mainLightColor + mainLightSpecular,
    //     _DirectLightIntensity * _MainLightIntensity);
    return (originalColor * mainLightColor + mainLightSpecular) * _DirectLightIntensity * _MainLightIntensity;
}

#endif
