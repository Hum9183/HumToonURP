#ifndef HT_NORMAL_OVERRIDE_INCLUDED
#define HT_NORMAL_OVERRIDE_INCLUDED

// 法線をオーバーライドする処理（オブジェクト空間で直接オーバーライド）
// マスクマップで指定された領域の法線を、オブジェクト空間で指定された方向にオーバーライドします

// 頂点シェーダー用（GI計算用）
// SAMPLE_TEXTURE2D_LODを使用
float3 ApplyNormalOverrideVS(float2 uv, float3 normalWS)
{
    // マスクマップから対象領域を取得（R チャンネルを使用、mipLevel=0）
    // テクスチャ専用のサンプラーを使用（sampler_NormalOverrideMask）
    half mask = SAMPLE_TEXTURE2D_LOD(_NormalOverrideMask, sampler_NormalOverrideMask, uv, 0).r;

    // マスク値と強度を掛け合わせる
    half blendFactor = mask * _NormalOverrideIntensity;

    if (blendFactor > 0.0)
    {
        // ワールド空間の法線をオブジェクト空間に変換
        float3 normalOS = TransformWorldToObjectNormal(normalWS);

        // オブジェクト空間でオーバーライド方向と元の法線をブレンド
        // _NormalOverrideDirection: オブジェクト空間での方向
        // 例: (0,0,1) = モデルの前方（ローカルZ軸）
        float3 overrideDirectionOS = normalize(_NormalOverrideDirection.xyz);
        normalOS = normalize(lerp(normalOS, overrideDirectionOS, blendFactor));

        // オブジェクト空間の法線をワールド空間に戻す
        return TransformObjectToWorldNormal(normalOS);
    }

    return normalWS;
}

// フラグメントシェーダー用（Normal Map適用後に使用）
// SAMPLE_TEXTURE2Dを使用
float3 ApplyNormalOverrideFS(float2 uv, float3 normalWS)
{
    // マスクマップから対象領域を取得（R チャンネルを使用）
    half mask = SAMPLE_TEXTURE2D(_NormalOverrideMask, sampler_NormalOverrideMask, uv).r;

    // マスク値と強度を掛け合わせる
    half blendFactor = mask * _NormalOverrideIntensity;

    if (blendFactor > 0.0)
    {
        // ワールド空間の法線をオブジェクト空間に変換
        float3 normalOS = TransformWorldToObjectNormal(normalWS);

        // オブジェクト空間でオーバーライド方向と元の法線をブレンド
        float3 overrideDirectionOS = normalize(_NormalOverrideDirection.xyz);
        normalOS = normalize(lerp(normalOS, overrideDirectionOS, blendFactor));

        // オブジェクト空間の法線をワールド空間に戻す
        return TransformObjectToWorldNormal(normalOS);
    }

    return normalWS;
}

// フラグメントシェーダー用（Override優先実装）
// Normal Map適用後に、Vertex Shaderのオーバーライド結果とブレンド
// SAMPLE_TEXTURE2Dを使用
float3 BlendNormalToVertexOverride(float2 uv, float3 normalMapResultWS, float3 vertexOverrideWS)
{
    // マスクマップから対象領域を取得（R チャンネルを使用）
    half mask = SAMPLE_TEXTURE2D(_NormalOverrideMask, sampler_NormalOverrideMask, uv).r;

    // マスク値と強度を掛け合わせる
    half blendFactor = mask * _NormalOverrideIntensity;

    if (blendFactor > 0.0)
    {
        // Normal Map結果とVertex Shaderのオーバーライド済み法線をブレンド
        // blendFactor = 1.0 でVertex Shaderのオーバーライドを完全に優先（Normal Map無視）
        return normalize(lerp(normalMapResultWS, vertexOverrideWS, blendFactor));
    }

    return normalMapResultWS;
}

#endif

