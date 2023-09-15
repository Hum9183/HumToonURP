#ifndef HUM_TOON_VARIABLES_PER_MATERIAL_INCLUDED
#define HUM_TOON_VARIABLES_PER_MATERIAL_INCLUDED

// Shade
half4 _FirstShadeColor;
half _FirstShadeBorderPos;
half _FirstShadeBorderBlur;

// MatCap
float4 _MatCapMap_ST;
half4 _MatCapColor;

// Light
half _MainLightColorWeight;
half _UseMainLightUpperLimit;
half _MainLightUpperLimit;
half _UseMainLightLowerLimit;
half _MainLightLowerLimit;
half _AdditionalLightsColorWeight;

#endif
