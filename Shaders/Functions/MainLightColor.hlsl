#ifndef MAIN_LIGHT_COLOR_INCLUDED
#define MAIN_LIGHT_COLOR_INCLUDED

half3 CalcMainLightColor(Light mainLight)
{
    half3 finalMainLightColor = 0;

#if defined(_LIGHT_LAYERS)
    uint meshRenderingLayers = GetMeshRenderingLayer();
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
#endif
    {
        finalMainLightColor = mainLight.color;
        finalMainLightColor = lerp(finalMainLightColor, min(finalMainLightColor, _MainLightUpperLimit), _UseMainLightUpperLimit);
        finalMainLightColor = lerp(finalMainLightColor, max(finalMainLightColor, _MainLightLowerLimit), _UseMainLightLowerLimit);
    }

    return finalMainLightColor;
}

half3 MixMainLightColor(half3 originalColor, half3 mainLightColor)
{
    return lerp(originalColor, originalColor * mainLightColor, _MainLightColorWeight);
}

#endif
