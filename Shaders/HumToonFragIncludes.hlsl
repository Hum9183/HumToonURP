#ifndef HUM_TOON_FRAG_INCLUDES_INCLUDED
#define HUM_TOON_FRAG_INCLUDES_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#include "Functions/HumToonBase.hlsl"
#include "Functions/HumToonShade.hlsl"

#if defined(_USE_RIM_LIGHT)
    #include "Functions/HumToonRimLight.hlsl"
#endif

#if defined(_USE_MAT_CAP)
    #include "Functions/HumToonMatCap.hlsl"
#endif

#include "Functions/MainLightColor.hlsl"

#if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX)
    #include "Functions/AdditionalLightsColor.hlsl"
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
