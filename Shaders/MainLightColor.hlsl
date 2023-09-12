#ifndef MAIN_LIGHT_COLOR_INCLUDED
#define MAIN_LIGHT_COLOR_INCLUDED

half3 CalcMainLightColor(half3 mainLightColor)
{
    half3 finalMainLightColor = mainLightColor * _MainLightColorWeight;
    finalMainLightColor = lerp(finalMainLightColor, min(finalMainLightColor, _MainLightUpperLimit), _UseMainLightUpperLimit);
    finalMainLightColor = lerp(finalMainLightColor, max(finalMainLightColor, _MainLightLowerLimit), _UseMainLightLowerLimit);
    return finalMainLightColor;
}

#endif
