#ifndef HUM_TOON_MAT_CAP_INCLUDED
#define HUM_TOON_MAT_CAP_INCLUDED

half3 CalcMatCap(float3 normalWS)
{
    // TODO:
    // MainLightの影響
    // その他、いろいろなオプション
    float3 matCapNormal = mul((float3x3)UNITY_MATRIX_V, normalWS);
    float2 matCapUV = matCapNormal.xy * 0.5 + 0.5;
    half3 matCapMapColor = SAMPLE_TEXTURE2D(_MatCapMap, sampler_BaseMap, matCapUV).rgb;
    return matCapMapColor * _MatCapColor;
}

#endif
