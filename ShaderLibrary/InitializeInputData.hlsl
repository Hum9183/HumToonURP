#ifndef INITISLIZE_INPUT_DATA_INCLUDED
#define INITISLIZE_INPUT_DATA_INCLUDED

void InitializeInputData(Varyings input, out InputData inputData)
{
    inputData = (InputData)0;

    #if defined(DEBUG_DISPLAY)
        inputData.positionWS = input.positionWS;
        inputData.normalWS = input.normalWS;
        inputData.viewDirectionWS = input.viewDirWS;
    #else
        inputData.positionWS = float3(0, 0, 0);
        inputData.normalWS = half3(0, 0, 1);
        inputData.viewDirectionWS = half3(0, 0, 1);
    #endif
    inputData.shadowCoord = 0;
    inputData.fogCoord = 0;
    inputData.vertexLighting = half3(0, 0, 0);
    inputData.bakedGI = half3(0, 0, 0);
    inputData.normalizedScreenSpaceUV = 0;
    inputData.shadowMask = half4(1, 1, 1, 1);
}

#endif
