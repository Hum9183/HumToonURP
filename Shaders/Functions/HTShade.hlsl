#ifndef HT_SHADE_INCLUDED
#define HT_SHADE_INCLUDED

#include "..\HTPredefine.hlsl"
#include "..\..\ShaderLibrary\HTUtils.hlsl"

half3 HTMixShade(half3 baseColor, float3 normalWS, float3 lightDirWS, half shadowAttenuation)
{
    const half3 shadeColor = 0;
    half halfLambert = HTCalcHalfLambert(normalWS, lightDirWS);
    half shade = HTBlurStep(_ShadeBorderPos, _ShadeBorderBlur, halfLambert);
    shade *= shadowAttenuation;
    return lerp(shadeColor, baseColor, shade);
}

#endif
