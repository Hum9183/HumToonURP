#ifndef HUM_TOON_ATTRIBUTES_INCLUDED
#define HUM_TOON_ATTRIBUTES_INCLUDED

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;

    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

#endif
