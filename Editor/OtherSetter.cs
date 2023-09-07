using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class OtherSetter
    {
        public static void Set(Material material, bool isOpaque, bool alphaClip)
        {
            // Setup double sided GI based on Cull state
            material.doubleSidedGI = (RenderFace)material.GetFloat(HumToonPropertyNames.CullMode) != RenderFace.Front;

            // Emission
            MaterialEditor.FixupEmissiveFlag(material);
            bool shouldEmissionBeEnabled =
                (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
            // TODO: keywordがここになるのはよくない
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._EMISSION, shouldEmissionBeEnabled);
        }
    }
}
