#ifndef HUM_TOON_INPUT_INCLUDED
#define HUM_TOON_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
    #define _DETAIL
#endif

// NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
CBUFFER_START(UnityPerMaterial)
    float4 _BaseMap_ST;
    float4 _DetailAlbedoMap_ST;
    half4 _BaseColor;
    half4 _SpecColor;
    half4 _EmissionColor;
    half _Cutoff;
    half _Smoothness;
    half _Metallic;
    half _BumpScale;
    half _Parallax;
    half _OcclusionStrength;
    half _ClearCoatMask;
    half _ClearCoatSmoothness;
    half _DetailAlbedoMapScale;
    half _DetailNormalMapScale;
    half _Surface;
CBUFFER_END

// NOTE: Do not ifdef the properties for dots instancing, but ifdef the actual usage.
// Otherwise you might break CPU-side as property constant-buffer offsets change per variant.
// NOTE: Dots instancing is orthogonal to the constant buffer above.
#ifdef UNITY_DOTS_INSTANCING_ENABLED
    UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
        UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
        UNITY_DOTS_INSTANCED_PROP(float4, _SpecColor)
        UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
        UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
        UNITY_DOTS_INSTANCED_PROP(float , _Smoothness)
        UNITY_DOTS_INSTANCED_PROP(float , _Metallic)
        UNITY_DOTS_INSTANCED_PROP(float , _BumpScale)
        UNITY_DOTS_INSTANCED_PROP(float , _Parallax)
        UNITY_DOTS_INSTANCED_PROP(float , _OcclusionStrength)
        UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatMask)
        UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatSmoothness)
        UNITY_DOTS_INSTANCED_PROP(float , _DetailAlbedoMapScale)
        UNITY_DOTS_INSTANCED_PROP(float , _DetailNormalMapScale)
        UNITY_DOTS_INSTANCED_PROP(float , _Surface)
    UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

    #define _BaseColor              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
    #define _SpecColor              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _SpecColor)
    #define _EmissionColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _EmissionColor)
    #define _Cutoff                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Cutoff)
    #define _Smoothness             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Smoothness)
    #define _Metallic               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Metallic)
    #define _BumpScale              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _BumpScale)
    #define _Parallax               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Parallax)
    #define _OcclusionStrength      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _OcclusionStrength)
    #define _ClearCoatMask          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ClearCoatMask)
    #define _ClearCoatSmoothness    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ClearCoatSmoothness)
    #define _DetailAlbedoMapScale   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _DetailAlbedoMapScale)
    #define _DetailNormalMapScale   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _DetailNormalMapScale)
    #define _Surface                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Surface)
#endif

TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

#ifdef _SPECULAR_SETUP
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
#else
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
#endif

#include "HumToonFunc.hlsl"

#endif
