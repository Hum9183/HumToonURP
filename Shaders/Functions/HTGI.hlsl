#ifndef HT_GI_INCLUDED
#define HT_GI_INCLUDED

half3 HTCalcIndirect(
    BRDFData brdfData, half3 bakedGI, half occlusion, float3 positionWS,
    half3 normalWS, half3 viewDirectionWS, float2 normalizedScreenSpaceUV)
{
    half3 reflectionVector = reflect(-viewDirectionWS, normalWS);
    half NoV = saturate(dot(normalWS, viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = 0;
#if defined(_HT_RECEIVE_INDIRECT_DIFFUSE)
    indirectDiffuse = bakedGI;
    indirectDiffuse *= _IndirectDiffuseIntensity;
#endif

    half3 indirectSpecular = 0;
#if defined(_HT_RECEIVE_INDIRECT_SPECULAR)
    indirectSpecular = GlossyEnvironmentReflection(reflectionVector, positionWS, brdfData.perceptualRoughness, 1.0h, normalizedScreenSpaceUV);
    indirectSpecular *= _IndirectSpecularIntensity;
#endif

    half3 indirect = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);

    if (IsOnlyAOLightingFeatureEnabled())
    {
        indirect = half3(1,1,1); // "Base white" for AO debug lighting mode
    }

    return indirect * occlusion * _IndirectLightIntensity;
}

#endif
