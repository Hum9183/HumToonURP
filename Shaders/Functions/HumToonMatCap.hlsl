#ifndef HUM_TOON_MAT_CAP_INCLUDED
#define HUM_TOON_MAT_CAP_INCLUDED

#include "../../ShaderLibrary/Func.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

// upがforwardに対して直角になるように調整する
float3 HumNormalizeUpViewDir(float3 up, float3 forward)
{
    float FdotU = dot(forward, up);
    float3 diff = forward * FdotU;
    return normalize(up - diff);
}

// パース補正とカメラのZ回転補正を適用したUNITY_MATRIX_Vを計算する
float2x3 HumCalcFixedMatCapViewMatrix(float3 viewDirWS)
{
    // NOTE: VD...View Direction

    float3 UP = float3(0, 1, 0);

    float3 forwardVD = HumIsPerspective() && _MatCapCorrectPerspectiveDistortion ? viewDirWS : HumGetCameraForwardVector();
    float3 upVD = _MatCapStabilizeCameraZRotation ? UP : HumGetCameraUpVector();
    upVD = HumNormalizeUpViewDir(upVD, forwardVD);
    float3 rightVD = cross(forwardVD, upVD);

    float2x3 fixedVD = float2x3(rightVD,// Simple: UNITY_MATRIX_V._m00_m01_m02
                                upVD);  // Simple: UNITY_MATRIX_V._m10_m11_m12
                                        // NOTE: MatCapUVにzは不要
    return fixedVD;
}

// MatCap用のUVを計算する
float2 HumCalcMatCapUV(float3 normalWS, float3 viewDirWS)
{

    float2x3 fixedVD = HumCalcFixedMatCapViewMatrix(viewDirWS);

    float2 matCapUV = mul(fixedVD, normalWS).xy;
    matCapUV = matCapUV * _MatCapMap_ST.xy + _MatCapMap_ST.zw;
    matCapUV = RemapZeroToOneRange(matCapUV);
    return matCapUV;
}

half3 HumCalcMatCap(float3 normalWS, float3 viewDirWS, half3 mainLightColor)
{
    // TODO:
    // ・Blur
    // ・Scale
    // ・Rotate
    // ・ブレンド方法(乗算etc.)
    // ・Mask

    float2 matCapUV = HumCalcMatCapUV(normalWS, viewDirWS);

    half3 matCapMapColor = SAMPLE_TEXTURE2D(_MatCapMap, sampler_BaseMap, matCapUV).rgb;
    matCapMapColor *= _MatCapColor * _MatCapIntensity;
    matCapMapColor = lerp(matCapMapColor, matCapMapColor * mainLightColor, _MatCapMainLightEffectiveness);
    return matCapMapColor;
}

#endif
