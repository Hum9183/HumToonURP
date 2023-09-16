#ifndef HUM_TOON_MAT_CAP_INCLUDED
#define HUM_TOON_MAT_CAP_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

half3 CalcMatCap(float3 normalWS, half3 mainLightColor)
{
    // TODO:
    // ・Blur
    // ・Scale
    // ・Rotate
    // ・スタビライザ
    // ・ブレンド方法(乗算etc.)
    // ・Persp or Ortho
    // ・Mask

    float2 matCapUV = mul((float3x3)UNITY_MATRIX_V, normalWS).xy;
    matCapUV = matCapUV * _MatCapMap_ST.xy + _MatCapMap_ST.zw;
    matCapUV = matCapUV * 0.5 + 0.5;

    half3 matCapMapColor = SAMPLE_TEXTURE2D(_MatCapMap, sampler_BaseMap, matCapUV).rgb;
    matCapMapColor *= _MatCapColor;
    matCapMapColor = lerp(matCapMapColor, matCapMapColor * mainLightColor, _MatCapMainLightEffectiveness);
    return matCapMapColor;
}

#endif
