using Hum.HumToon.Editor.Utils;
using UnityEngine;

namespace Hum.HumToon.Editor.Archives.LitBased
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
