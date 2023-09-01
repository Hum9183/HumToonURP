using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class PassSetter
    {
        public static void Set(Material material, bool isOpaque)
        {
            // Transparent
            material.SetShaderPassEnabled("ShadowCaster", isOpaque);

            // Depth
            material.SetShaderPassEnabled("DepthOnly", isOpaque);
        }
    }
}
