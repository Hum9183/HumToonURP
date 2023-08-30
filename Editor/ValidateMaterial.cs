
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

            SetMaterialKeywords(material);
            LitGUI.SetMaterialKeywords(material);
            LitDetailGUI.SetMaterialKeywords(material);
        }


        /// <summary>
        /// this function is shared with ShaderGraph Lit/Unlit GUIs and also the hand-written GUIs
        /// </summary>
        private static void UpdateMaterialSurfaceOptions(Material material, int automaticRenderQueue)
        {
            if (material.renderQueue != automaticRenderQueue)
                material.renderQueue = automaticRenderQueue;

            // Receive Shadows
            bool receiveShadows = material.GetFloat(HumToonPropertyNames.ReceiveShadows).ToBool();
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, receiveShadows is false);
        }

        /// <summary>
        /// Sets up the keywords for the material and shader.
        /// </summary>
        /// <param name="material">The material to use.</param>
        /// <param name="shadingModelFunc">Function to set shading models.</param>
        /// <param name="shaderFunc">Function to set some extra shader parameters.</param>
        private static void SetMaterialKeywords(Material material)
        {
            // Setup double sided GI based on Cull state
            material.doubleSidedGI = (RenderFace)material.GetFloat(HumToonPropertyNames.CullMode) != RenderFace.Front;

            // Temporary fix for lightmapping. TODO: to be replaced with attribute tag.
            material.SetTexture("_MainTex", material.GetTexture("_BaseMap"));
            material.SetTextureScale("_MainTex", material.GetTextureScale("_BaseMap"));
            material.SetTextureOffset("_MainTex", material.GetTextureOffset("_BaseMap"));
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
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, material.GetTexture("_BumpMap"));
        }
    }
}
