
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        /// <summary>
        /// Called when a material has been changed.
        /// </summary>
        public override void ValidateMaterial(Material material)
        {
            Debug.Log("ValidateMaterial");

            MaterialBlendModeSetter.Setup(material, out int automaticRenderQueue);
            UpdateMaterialSurfaceOptions(material, automaticRenderQueue);
            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
        }


        /// <summary>
        /// this function is shared with ShaderGraph Lit/Unlit GUIs and also the hand-written GUIs
        /// </summary>
        private static void UpdateMaterialSurfaceOptions(Material material, int automaticRenderQueue)
        {
            // apply automatic render queue
            if (automaticRenderQueue != material.renderQueue)
                material.renderQueue = automaticRenderQueue;

            // Cast Shadows (set based on transparency)
            bool castShadows = IsOpaque(material);
            material.SetShaderPassEnabled("ShadowCaster", castShadows);

            // Receive Shadows
            bool receiveShadows = material.GetFloat(HumToonPropertyNames.ReceiveShadows).ToBool();
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, receiveShadows is false);
        }

        // this is the function used by Lit.shader, Unlit.shader GUIs
        /// <summary>
        /// Sets up the keywords for the material and shader.
        /// </summary>
        /// <param name="material">The material to use.</param>
        /// <param name="shadingModelFunc">Function to set shading models.</param>
        /// <param name="shaderFunc">Function to set some extra shader parameters.</param>
        private static void SetMaterialKeywords(Material material, Action<Material> shadingModelFunc = null, Action<Material> shaderFunc = null)
        {
            // Setup double sided GI based on Cull state
            if (material.HasProperty(HumToonPropertyNames.CullMode))
                material.doubleSidedGI = (RenderFace)material.GetFloat(HumToonPropertyNames.CullMode) != RenderFace.Front;

            // Temporary fix for lightmapping. TODO: to be replaced with attribute tag.
            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", material.GetTexture("_BaseMap"));
                material.SetTextureScale("_MainTex", material.GetTextureScale("_BaseMap"));
                material.SetTextureOffset("_MainTex", material.GetTextureOffset("_BaseMap"));
            }
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", material.GetColor("_BaseColor"));

            // Emission
            if (material.HasProperty(HumToonPropertyNames.EmissionColor))
                MaterialEditor.FixupEmissiveFlag(material);

            bool shouldEmissionBeEnabled =
                (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;

            // Not sure what this is used for, I don't see this property declared by any Unity shader in our repo...
            // I'm guessing it is some kind of legacy material upgrade support thing?  Or maybe just dead code now...
            if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
                shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;

            CoreUtils.SetKeyword(material, ShaderKeywordStrings._EMISSION, shouldEmissionBeEnabled);

            // Normal Map
            if (material.HasProperty("_BumpMap"))
                CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, material.GetTexture("_BumpMap"));

            // Shader specific keyword functions
            shadingModelFunc?.Invoke(material);
            shaderFunc?.Invoke(material);
        }
    }
}
