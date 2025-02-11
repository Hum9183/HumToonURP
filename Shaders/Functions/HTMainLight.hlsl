#ifndef HT_MAIN_LIGHT_COLOR_INCLUDED
#define HT_MAIN_LIGHT_COLOR_INCLUDED

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
        mainLightDiffuse = lerp(mainLightDiffuse, min(mainLightDiffuse, _MainLightUpperLimit), _UseMainLightUpperLimit);
        mainLightDiffuse = lerp(mainLightDiffuse, max(mainLightDiffuse, _MainLightLowerLimit), _UseMainLightLowerLimit);

        // NOTE:
        // DirectionalLightのIntensityが0になるとLightLayerからいなくなるらしいため、
        // 上限と下限の処理は、_LIGHT_LAYERSのifの外で計算しても良いかもしれない。
    }

    return mainLightDiffuse;
}

half3 HTMainLightSpecular(
    BRDFData brdfData, Light mainLight, half3 normalWS, half3 viewDirectionWS
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
)
{
    half3 mainLightSpecular = 0;

#if defined(_HT_USE_MAIN_LIGHT_SPECULAR)
    #if defined(_LIGHT_LAYERS)
        if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
    #endif
        {
            mainLightSpecular = DirectBRDFSpecular(brdfData, normalWS, mainLight.direction, viewDirectionWS);
            mainLightSpecular *= brdfData.specular * mainLight.color * _MainLightSpecularIntensity;
        }
#endif

    return mainLightSpecular;
}

half3 HTMixMainLight(half3 originalColor, half3 mainLightColor, half3 mainLightSpecular)
{
    return lerp(
        originalColor,
        originalColor * mainLightColor + mainLightSpecular,
        _MainLightColorWeight);
}

#endif
