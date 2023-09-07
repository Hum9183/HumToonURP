using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class FloatSetter
    {
        public static void Set(Material material, bool isOpaque, bool alphaClip)
        {
            material.SetFloat(HumToonPropertyNames.AlphaToMask, alphaClip.ToFloat());

            material.SetFloat(HumToonPropertyNames.ZWrite, isOpaque.ToFloat());
        }
    }
}
