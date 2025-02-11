#ifndef HT_MAT_CAP_INCLUDED
#define HT_MAT_CAP_INCLUDED

#include "..\..\ShaderLibrary\HTUtils.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

// upがforwardに対して直角になるように調整する
float3 HTNormalizeUpViewDir(float3 up, float3 forward)
{
    float FdotU = dot(forward, up);
    float3 diff = forward * FdotU;
    return normalize(up - diff);
}

// パース補正とカメラのZ回転補正を適用したUNITY_MATRIX_Vを計算する
float2x3 HTCalcFixedMatCapViewMatrix(float3 viewDirWS)
{
    // NOTE: VD...View Direction

    float3 UP = float3(0, 1, 0);

    float3 forwardVD = HTIsPerspective() && _MatCapCorrectPerspectiveDistortion ? viewDirWS : HTGetCameraForwardVector();
    float3 upVD = _MatCapStabilizeCameraZRotation ? UP : HTGetCameraUpVector();
    upVD = HTNormalizeUpViewDir(upVD, forwardVD);
    float3 rightVD = cross(forwardVD, upVD);

    float2x3 fixedVD = float2x3(rightVD,// Simple: UNITY_MATRIX_V._m00_m01_m02
                                upVD);  // Simple: UNITY_MATRIX_V._m10_m11_m12
                                        // NOTE: MatCapUVにzは不要
    return fixedVD;
}

// MatCap用のUVを計算する
float2 HTCalcMatCapUV(float3 normalWS, float3 viewDirWS)
{

    float2x3 fixedVD = HTCalcFixedMatCapViewMatrix(viewDirWS);

    float2 matCapUV = mul(fixedVD, normalWS).xy;
    matCapUV = matCapUV * _MatCapMap_ST.xy + _MatCapMap_ST.zw;
    matCapUV = HTRemapZeroToOneRange(matCapUV);
    return matCapUV;
}

half3 HTCalcMatCapColor(float3 normalWS, float3 viewDirWS, half3 mainLightColor
#if defined(_HT_USE_MAT_CAP_MASK)
    , float2 uvForMask
#endif
)
{
    // TODO:
    // ・Scale
    // ・Rotate
    // ・ブレンド方法(乗算etc.)

    float2 matCapUV = HTCalcMatCapUV(normalWS, viewDirWS);

    half3 matCapColor = _MatCapColor;
    matCapColor *= SAMPLE_TEXTURE2D_LOD(_MatCapMap, sampler_BaseMap, matCapUV, _MatCapMapMipLevel).rgb;
#if defined(_HT_USE_MAT_CAP_MASK)
    matCapColor *= lerp(ONE, SAMPLE_TEXTURE2D(_MatCapMask, sampler_BaseMap, uvForMask).r, _MatCapMaskIntensity);
#endif
    matCapColor = lerp(matCapColor, matCapColor * mainLightColor, _MatCapMainLightEffectiveness);
    return matCapColor * _MatCapIntensity;
}

#endif
