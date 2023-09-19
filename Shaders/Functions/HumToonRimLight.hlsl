#ifndef HUM_TOON_RIM_LIGHT_INCLUDED
#define HUM_TOON_RIM_LIGHT_INCLUDED

#include "../../ShaderLibrary/Func.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

half3 HumCalcRimLight(float3 normalWS, float3 viewDirWS, half3 mainLightColor)
{
    // TODO:
    // ・ブレンド方法(乗算etc.)
    // ・Mask
    half NdotV = dot(normalWS, viewDirWS);
    return OneMinus(NdotV);
}

#endif
