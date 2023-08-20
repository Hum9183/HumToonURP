#ifndef HUM_TOON_VERT_INCLUDED
#define HUM_TOON_VERT_INCLUDED

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

    output.positionCS = vertexInput.positionCS;
    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
    #if defined(_FOG_FRAGMENT)
        output.fogCoord = vertexInput.positionVS.z;
    #else
        output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
    #endif

    #if defined(DEBUG_DISPLAY)
        // normalWS and tangentWS already normalize.
        // this is required to avoid skewing the direction during interpolation
        // also required for per-vertex lighting and SH evaluation
        VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
        half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);

        // already normalized from normal transform to WS.
        output.positionWS = vertexInput.positionWS;
        output.normalWS = normalInput.normalWS;
        output.viewDirWS = viewDirWS;
    #endif

    return output;
}

#endif
