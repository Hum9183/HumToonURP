#ifndef FUNC_INCLUDED
#define FUNC_INCLUDED

float HalfLambert(float3 normalWS, float3 lightDir)
{
    float NdotL = dot(normalWS, lightDir);
    return NdotL * 0.5 + 0.5;
}

#endif
