using System;
using UnityEngine;
using UnityEditor;
using Unity.Rendering.Universal;
using UnityEditor.ShaderGraph;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEditor.Rendering.Universal.ShaderGUI;

namespace HumToon.Editor
{
    public static class Utils
    {
        public static bool IsOpaque(Material material)
        {
            return (SurfaceType)material.GetFloat(HumToonPropertyNames.SurfaceType) is SurfaceType.Opaque;
        }

        /// <summary>
        /// this function is shared with ShaderGraph Lit/Unlit GUIs and also the hand-written GUIs
        /// </summary>
        public static void UpdateMaterialRenderQueue(Material material, int renderQueue)
        {
            if (material.renderQueue != renderQueue)
                material.renderQueue = renderQueue;
        }

    }
}
