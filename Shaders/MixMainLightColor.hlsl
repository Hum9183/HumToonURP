#ifndef MIX_MAIN_LIGHT_COLOR_INCLUDED
#define MIX_MAIN_LIGHT_COLOR_INCLUDED

half3 MixMainLightColor(half3 originalColor, half3 mainLightColor)
{
    return lerp(originalColor, originalColor * mainLightColor, _MainLightColorWeight);
}

#endif
