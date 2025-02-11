#ifndef HT_INITIALIZE_SURFACE_DATA_INCLUDED
#define HT_INITIALIZE_SURFACE_DATA_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

void InitializeSurfaceData(out SurfaceData surfaceData)
{
    surfaceData = (SurfaceData)0;

    surfaceData.albedo = half3(1, 1, 1);
    surfaceData.alpha = 1;
    surfaceData.emission = 0;
    surfaceData.metallic = 0;
    surfaceData.occlusion = 1;
    surfaceData.smoothness = 1;
    surfaceData.specular = 0;
    surfaceData.clearCoatMask = 0;
    surfaceData.clearCoatSmoothness = 1;
    surfaceData.normalTS = half3(0, 0, 1);
}

#endif
