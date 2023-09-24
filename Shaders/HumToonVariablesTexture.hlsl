#ifndef HUM_TOON_VARIABLES_TEXTURE_INCLUDED
#define HUM_TOON_VARIABLES_TEXTURE_INCLUDED

// Shade
#if defined(_HUM_USE_FIRST_SHADE_MAP)
    TEXTURE2D(_FirstShadeMap);
#endif

#if defined(_HUM_USE_SECOND_SHADE_MAP)
    TEXTURE2D(_SecondShadeMap);
#endif

#if defined(_HUM_USE_RAMP_SHADE)
    TEXTURE2D(_RampShadeMap);
    SAMPLER(hum_sampler_linear_clamp);
#endif

#if defined(_HUM_USE_SHADE_CONTROL_MAP)
    TEXTURE2D(_ShadeControlMap);
#endif

// RimLight
#if defined(_HUM_USE_RIM_LIGHT)
    TEXTURE2D(_RimLightMap);
#endif

// Emission
#if defined(_HUM_USE_EMISSION)
    TEXTURE2D(_EmissionMap);
#endif

// MatCap
#if defined(_HUM_USE_MAT_CAP)
    TEXTURE2D(_MatCapMap);
#endif

#endif
