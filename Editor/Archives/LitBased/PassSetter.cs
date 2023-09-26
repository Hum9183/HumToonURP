using UnityEngine;

namespace Hum.HumToon.Editor.Archives.LitBased
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
