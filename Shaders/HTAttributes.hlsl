#ifndef HT_ATTRIBUTES_INCLUDED
#define HT_ATTRIBUTES_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

struct Attributes
{
    float4 positionOS        : POSITION;
    float3 normalOS          : NORMAL;
    float4 tangentOS         : TANGENT;
    float2 texcoord          : TEXCOORD0;
    float2 staticLightmapUV  : TEXCOORD1;
    float2 dynamicLightmapUV : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

#endif
