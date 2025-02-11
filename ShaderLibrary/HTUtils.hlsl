#ifndef HT_UTILS_INCLUDED
#define HT_UTILS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

half HTCalcHalfLambert(float3 normalWS, float3 lightDirWS)
{
    half NdotL = dot(normalWS, lightDirWS);
    return NdotL * 0.5 + 0.5;
}

inline half HTOneMinus(half value)
{
    return 1 - value;
}

inline float HTOneMinus(float value)
{
    return 1 - value;
}

inline float2 HTRemapZeroToOneRange(float2 value)
{
    return value * 0.5 + 0.5;
}

inline float1 HTRemapZeroToOneRange(float1 value)
{
    return value * 0.5 + 0.5;
}

inline half HTBlurStep(half step, half blur, half inValue)
{
    return smoothstep(step - blur, step + blur, inValue);
}


float3 HTGetCameraForwardVector()
{
#if defined(USING_STEREO_MATRICES)
    return normalize(unity_StereoMatrixV[0]._m20_m21_m22 + unity_StereoMatrixV[1]._m20_m21_m22);
#else
    return UNITY_MATRIX_V._m20_m21_m22;
#endif
}

float3 HTGetCameraUpVector()
{
    return UNITY_MATRIX_V._m10_m11_m12;
}

float3 HTGetCameraRightVector()
{
#if defined(USING_STEREO_MATRICES)
    return cross(HTGetCameraForwardVector(), HTGetCameraUpVector());
#else
    return UNITY_MATRIX_V._m00_m01_m02;
#endif
}

bool HTIsPerspective()
{
#if defined(HUM_HDRP) && defined(SHADERPASS) && (SHADERPASS == SHADERPASS_SHADOWS)
    return UNITY_MATRIX_P._m33 == 0;
#else
    return unity_OrthoParams.w == 0;
#endif
}

#endif
