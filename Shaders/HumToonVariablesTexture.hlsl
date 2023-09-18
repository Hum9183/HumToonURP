#ifndef HUM_TOON_VARIABLES_TEXTURE_INCLUDED
#define HUM_TOON_VARIABLES_TEXTURE_INCLUDED

// Shade
#if defined(_USE_FIRST_SHADE_MAP)
    TEXTURE2D(_FirstShadeMap);
#endif

#if defined(_USE_SECOND_SHADE_MAP)
    TEXTURE2D(_SecondShadeMap);
#endif

// MatCap
#if defined(_USE_MAT_CAP)
    TEXTURE2D(_MatCapMap);
#endif

#endif
