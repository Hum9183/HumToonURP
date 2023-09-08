#ifndef RENDERING_LAYERS_INCLUDED
#define RENDERING_LAYERS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

void SetRenderingLayers(out float4 outRenderingLayers)
{
    uint renderingLayers = GetMeshRenderingLayer();
    outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
}

#endif
