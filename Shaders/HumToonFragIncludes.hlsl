#ifndef HUM_TOON_FRAG_INCLUDES_INCLUDED
#define HUM_TOON_FRAG_INCLUDES_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#include "HumToonPredefine.hlsl"

#if NOT(defined(_HUM_USE_FIRST_SHADE_MAP)) || NOT(defined(_HUM_USE_SECOND_SHADE_MAP)) || defined(_HUM_USE_EX_FIRST_SHADE)
    #define _HUM_REQUIRES_BASE_MAP_COLOR_ONLY
#endif

// Features include
#include "Functions/HumToonBase.hlsl"

#if defined(_HUM_USE_FIRST_SHADE) || defined(_HUM_USE_SECOND_SHADE) || defined(_HUM_USE_RAMP_SHADE)
    #include "Functions/HumToonShade.hlsl"
#endif

#if defined(_LIGHT_COOKIES) && defined(_HUM_USE_MAIN_LIGHT_COOKIE_AS_SHADE)
    #include "../ShaderLibrary/HumRealtimeLights.hlsl"
#endif

#if defined(_HUM_USE_RIM_LIGHT)
    #include "Functions/HumToonRimLight.hlsl"
#endif

#if defined(_HUM_USE_EMISSION)
    #include "Functions/HumToonEmission.hlsl"
#endif

#if defined(_HUM_USE_MAT_CAP)
    #include "Functions/HumToonMatCap.hlsl"
#endif

#include "Functions/MainLightColor.hlsl"

#include "Functions/HumToonGI.hlsl"

#if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX)
    #include "Functions/HumToonAdditionalLightsColor.hlsl"
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
