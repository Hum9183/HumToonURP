#ifndef HT_MAIN_LIGHT_COLOR_INCLUDED
#define HT_MAIN_LIGHT_COLOR_INCLUDED

half3 HTCalcMainLightColor(
    Light mainLight
#if defined(_LIGHT_LAYERS)
    , uint meshRenderingLayers
#endif
)
{
    half3 mainLightColor = 0;

#if defined(_LIGHT_LAYERS)
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
#endif
    {
        mainLightColor = mainLight.color;
        mainLightColor = lerp(mainLightColor, min(mainLightColor, _MainLightUpperLimit), _UseMainLightUpperLimit);
        mainLightColor = lerp(mainLightColor, max(mainLightColor, _MainLightLowerLimit), _UseMainLightLowerLimit);

        // NOTE:
        // DirectionalLightのIntensityが0になるとLightLayerからいなくなるらしいため、
        // 上限と下限の処理は、_LIGHT_LAYERSのifの外で計算しても良いかもしれない。
    }

    return mainLightColor;
}

half3 HTMixMainLightColor(half3 originalColor, half3 mainLightColor)
{
    return lerp(originalColor, originalColor * mainLightColor, _MainLightColorWeight);
}

#endif
