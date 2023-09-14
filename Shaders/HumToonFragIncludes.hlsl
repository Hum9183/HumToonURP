#ifndef HUM_TOON_FRAG_INCLUDES_INCLUDED
#define HUM_TOON_FRAG_INCLUDES_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#include "HumToonBaseColor.hlsl"
#include "HumToonShade.hlsl"

#if defined(_USE_MAT_CAP)
    #include "HumToonMatCap.hlsl"
#endif

#include "MainLightColor.hlsl"

#if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX)
    #include "AdditionalLightsColor.hlsl"
#endif

#if defined(_WRITE_RENDERING_LAYERS)
    #include "../ShaderLibrary/RenderingLayers.hlsl"
#endif

#include "HumToonInput.hlsl"
#include "HumToonVaryings.hlsl"
#include "HumToonFunc.hlsl"
#include "InitializeInputData.hlsl"

#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

#endif
