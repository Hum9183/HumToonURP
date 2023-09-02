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

        public static bool GetPreserveSpecular(Material material, TransparentBlendMode transparentBlendMode)
        {
            // Lift alpha multiply from ROP to shader by setting pre-multiplied _SrcBlend mode.
            // The intent is to do different blending for diffuse and specular in shader.
            // ref: http://advances.realtimerendering.com/other/2016/naughty_dog/NaughtyDog_TechArt_Final.pdf
            return material.GetFloat(HumToonPropertyNames.BlendModePreserveSpecular).ToBool()
                   && transparentBlendMode != TransparentBlendMode.Multiply
                   && transparentBlendMode != TransparentBlendMode.Premultiply;
        }
    }
}
