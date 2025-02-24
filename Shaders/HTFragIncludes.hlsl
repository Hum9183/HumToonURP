#ifndef HT_FRAG_INCLUDES_INCLUDED
#define HT_FRAG_INCLUDES_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#include "HTPredefine.hlsl"

#if NOT(defined(_HT_USE_FIRST_SHADE_MAP)) || NOT(defined(_HT_USE_SECOND_SHADE_MAP)) || defined(_HT_USE_EX_FIRST_SHADE)
    #define _HT_REQUIRES_BASE_MAP_COLOR_ONLY
#endif

// Features include
#include "Functions/HTBase.hlsl"
#include "Functions/HTSsao.hlsl"

#if defined(_HT_USE_FIRST_SHADE) || defined(_HT_USE_SECOND_SHADE) || defined(_HT_USE_RAMP_SHADE)
    #include "Functions/HTShade.hlsl"
#endif

#if defined(_LIGHT_COOKIES) && defined(_HT_USE_MAIN_LIGHT_COOKIE_AS_SHADE)
    #include "../ShaderLibrary/HTRealtimeLights.hlsl"
#endif

#if defined(_HT_USE_RIM_LIGHT)
    #include "Functions/HTRimLight.hlsl"
#endif

#if defined(_HT_USE_EMISSION)
    #include "Functions/HTEmission.hlsl"
#endif

#if defined(_HT_USE_MAT_CAP)
    #include "Functions/HTMatCap.hlsl"
#endif

#include "Functions\HTMainLight.hlsl"

#include "Functions\HTGI.hlsl"

#if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX)
    #include "Functions/HTAdditionalLights.hlsl"
#endif

#if defined(_WRITE_RENDERING_LAYERS)
    #include "../ShaderLibrary/HTRenderingLayers.hlsl"
#endif

#include "HTInput.hlsl"
#include "HTVaryings.hlsl"
#include "HTFunc.hlsl"
#include "HTInitializeInputData.hlsl"

#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

#endif
