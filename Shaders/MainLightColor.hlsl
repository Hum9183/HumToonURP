#ifndef MAIN_LIGHT_COLOR_INCLUDED
#define MAIN_LIGHT_COLOR_INCLUDED

half3 CalcMainLightColor(half3 mainLightColor)
{
    return mainLightColor * _MainLightColorWeight;
}

#endif
