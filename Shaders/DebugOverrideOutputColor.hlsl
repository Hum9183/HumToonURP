#ifndef DEBUG_OVERRIDE_OUTPUT_COLOR
#define DEBUG_OVERRIDE_OUTPUT_COLOR

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

#include "InitializeSurfaceData.hlsl"

half4 DebugOverrideOutputColor(InputData inputData, half4 color)
{
    SurfaceData surfaceData;
    InitializeSurfaceData(surfaceData);
    surfaceData.albedo = color.rgb;
    surfaceData.alpha = color.a;

    half4 debugColor;
    if (CanDebugOverrideOutputColor(inputData, surfaceData, debugColor))
    {
        return debugColor;
    }
    return color;
}

#endif
