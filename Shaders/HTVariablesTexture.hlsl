#ifndef HT_VARIABLES_TEXTURE_INCLUDED
#define HT_VARIABLES_TEXTURE_INCLUDED

// Shade
#if defined(_HT_USE_FIRST_SHADE_MAP)
    TEXTURE2D(_FirstShadeMap);
#endif

#if defined(_HT_USE_SECOND_SHADE_MAP)
    TEXTURE2D(_SecondShadeMap);
#endif

#if defined(_HT_USE_RAMP_SHADE)
    TEXTURE2D(_RampShadeMap);
    SAMPLER(hum_sampler_linear_clamp);
#endif

#if defined(_HT_USE_SHADE_CONTROL_MAP)
    TEXTURE2D(_ShadeControlMap);
#endif

// RimLight
#if defined(_HT_USE_RIM_LIGHT)
    TEXTURE2D(_RimLightMap);
#endif

// Emission
#if defined(_HT_USE_EMISSION)
    TEXTURE2D(_EmissionMap);
#endif

// MatCap
#if defined(_HT_USE_MAT_CAP)
    TEXTURE2D(_MatCapMap);
#endif

#if defined(_HT_USE_MAT_CAP_MASK)
    TEXTURE2D(_MatCapMask);
#endif

#endif
