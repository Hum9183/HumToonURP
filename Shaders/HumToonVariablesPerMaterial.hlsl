#ifndef HUM_TOON_VARIABLES_PER_MATERIAL_INCLUDED
#define HUM_TOON_VARIABLES_PER_MATERIAL_INCLUDED

// Shade
half4 _FirstShadeColor;
half _FirstShadeBorderPos;
half _FirstShadeBorderBlur;

half _UseExFirstShade;
half4 _ExFirstShadeColor;
half _ExFirstShadeWidth;

half4 _SecondShadeColor;
half _SecondShadeBorderPos;
half _SecondShadeBorderBlur;

half _ShadeControlMapIntensity;

// RimLight
half4 _RimLightColor;
half _RimLightIntensity;
half _RimLightBorderPos;
half _RimLightBorderBlur;
half _RimLightMainLightEffectiveness;

// Emission
half3 _EmissionColor;
half _EmissionIntensity;
half _EmissionFactorR;
half _EmissionFactorG;
half _EmissionFactorB;

// MatCap
float4 _MatCapMap_ST;
half4 _MatCapColor;
half _MatCapIntensity;
half _MatCapMapMipLevel;
half _MatCapCorrectPerspectiveDistortion;
half _MatCapStabilizeCameraZRotation;
half _MatCapMaskIntensity;
half _MatCapMainLightEffectiveness;

// Light
half _MainLightColorWeight;
half _UseMainLightUpperLimit;
half _MainLightUpperLimit;
half _UseMainLightLowerLimit;
half _MainLightLowerLimit;
half _UseMainLightCookieAsShade;
half _AdditionalLightsColorWeight;

// GI
half _ReceiveGI;
half _GIColorWeight;

#endif
