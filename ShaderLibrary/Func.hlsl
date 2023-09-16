#ifndef FUNC_INCLUDED
#define FUNC_INCLUDED

float HalfLambert(float3 normalWS, float3 lightDir)
{
    float NdotL = dot(normalWS, lightDir);
    return NdotL * 0.5 + 0.5;
}

half CalcHalfLambert(float3 normalWS, float3 lightDirWS)
{
    half NdotL = dot(normalWS, lightDirWS);
    return NdotL * 0.5 + 0.5;
}

inline half OneMinus(half value)
{
    return 1 - value;
}

inline float OneMinus(float value)
{
    return 1 - value;
}

inline float2 RemapZeroToOneRange(float2 value)
{
    return value * 0.5 + 0.5;
}

#endif
