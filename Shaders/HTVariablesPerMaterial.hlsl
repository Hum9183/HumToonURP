#ifndef HT_VARIABLES_PER_MATERIAL_INCLUDED
#define HT_VARIABLES_PER_MATERIAL_INCLUDED

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

// Normal Override
float4 _NormalOverrideDirection;
half _NormalOverrideIntensity;

// Direct Lighting
half _DirectLightIntensity;

half _MainLightIntensity;
half _ReceiveMainLightDiffuse;
half _MainLightDiffuseIntensity;
half _ReceiveMainLightSpecular;
half _MainLightSpecularIntensity;

half _AdditionalLightsIntensity;
half _ReceiveAdditionalLightsDiffuse;
half _AdditionalLightsDiffuseIntensity;
half _ReceiveAdditionalLightsSpecular;
half _AdditionalLightsSpecularIntensity;

// Indirect Lighting(GI)
half _IndirectLightIntensity;

half _ReceiveIndirectDiffuse;
half _IndirectDiffuseIntensity;
half _ReceiveIndirectSpecular;
half _IndirectSpecularIntensity;

half _ReceiveSsao;
half _SsaoIntensity;
half _UseSsaoMask;
half _SsaoMaskIntensity;

#endif
