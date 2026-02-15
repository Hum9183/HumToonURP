#ifndef HT_FRAG_INCLUDED
#define HT_FRAG_INCLUDED

#include "HTCalcFragColor.hlsl"

void frag(
    Varyings input
    , out half4 outColor : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    // UV
    const float2 uv0 = input.uv;

    // SurfaceData
    SurfaceData surfaceData = (SurfaceData)0;
    InitializeStandardLitSurfaceData(uv0, surfaceData);

    // LOD Fade
#ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
#endif

    // InputData
    InputData inputData = (InputData)0;
    HTInitializeInputData(input, surfaceData.normalTS, inputData);

#if UNITY_VERSION >= 60000000 // URP内部デバッグマクロの変更対応 https://github.com/Unity-Technologies/Graphics/commit/64c1408
    SETUP_DEBUG_TEXTURE_DATA_FOR_TEX(inputData, uv0, _BaseMap);
#else
    SETUP_DEBUG_TEXTURE_DATA(inputData, uv0, _BaseMap);
#endif

    // TODO:
    // - openPBRをベースに処理を見直す
    // - 顔用シャドウマップ
    // - Varyings SurfaceData InputData BRDFDataの整理
    // - specular, roughness, metalnessなどの整備(TextureはLit合わせで作る)
    // - ior
    // - transmission
    // - subsurface
    // - sheen
    // - thin film
    // - inspectorでデフォルト値に戻せるようにする
    // - inspectorで不要なプロパティを削除できるようにする
    // - spotLightのSpecular計算がLitと違う気がする(Roughness違うから？)
    // - specularHighlightなどのキーワードをLitとどこまで共通化するか検討する(マテリアルの切替時のscriptで変えるのもありだが、Lib内のキーワード分岐の恩恵を受けられなくなる)

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    half4 finalColor = 0;
    finalColor = HTCalcFragColor(uv0, inputData, surfaceData);
    finalColor.rgb = MixFog(finalColor.rgb, inputData.fogCoord);
    finalColor.a = OutputAlpha(finalColor.a, IsSurfaceTypeTransparent(_SurfaceType));

    outColor = finalColor;

    // Rendering Layers
#ifdef _WRITE_RENDERING_LAYERS
    SetRenderingLayers(outRenderingLayers);
#endif
}

#endif
