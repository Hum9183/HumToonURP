using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using UnityEditor.Rendering.Universal.ShaderGUI;

namespace HumToon.Editor
{
    /// <summary>
    /// Editor script for the Lit material inspector.
    /// </summary>
    public static class LitGUI
    {
        private static bool ClearCoatEnabled(Material material)
        {
            return material.HasProperty("_ClearCoat") && material.GetFloat("_ClearCoat") > 0.0;
        }

        /// <summary>
        /// Retrieves the alpha channel used for smoothness.
        /// </summary>
        /// <param name="material"></param>
        /// <returns>The Alpha channel used for Smoothness.</returns>
        public static SmoothnessTextureChannel GetSmoothnessTextureChannel(Material material)
        {
            int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int)SmoothnessTextureChannel.AlbedoAlpha)
                return SmoothnessTextureChannel.AlbedoAlpha;

            return SmoothnessTextureChannel.SpecularMetallicAlpha;
        }

        // (shared by all lit shaders, including shadergraph Lit Target and Lit.shader)
        internal static void SetupSpecularWorkflowKeyword(Material material, out bool isSpecularWorkflow)
        {
            isSpecularWorkflow = false;     // default is metallic workflow
            if (material.HasProperty("_WorkflowMode"))
                isSpecularWorkflow = ((WorkflowMode)material.GetFloat("_WorkflowMode")) == WorkflowMode.Specular;
            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkflow);
        }

        /// <summary>
        /// Sets up the keywords for the Lit shader and material.
        /// </summary>
        /// <param name="material"></param>
        public static void SetMaterialKeywords(Material material)
        {
            SetupSpecularWorkflowKeyword(material, out bool isSpecularWorkFlow);

            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            var specularGlossMap = isSpecularWorkFlow ? "_SpecGlossMap" : "_MetallicGlossMap";
            var hasGlossMap = material.GetTexture(specularGlossMap) != null;

            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);

            if (material.HasProperty("_SpecularHighlights"))
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF",
                    material.GetFloat("_SpecularHighlights") == 0.0f);
            if (material.HasProperty("_EnvironmentReflections"))
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF",
                    material.GetFloat("_EnvironmentReflections") == 0.0f);
            if (material.HasProperty("_OcclusionMap"))
                CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));

            if (material.HasProperty("_ParallaxMap"))
                CoreUtils.SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));

            if (material.HasProperty("_SmoothnessMapChannel"))
            {
                var opaque = HumToonInspector.IsOpaque(material);
                CoreUtils.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                    GetSmoothnessTextureChannel(material) == SmoothnessTextureChannel.AlbedoAlpha && opaque);
            }

            // Clear coat keywords are independent to remove possiblity of invalid combinations.
            if (ClearCoatEnabled(material))
            {
                var hasMap = material.HasProperty("_ClearCoatMap") && material.GetTexture("_ClearCoatMap") != null;
                if (hasMap)
                {
                    CoreUtils.SetKeyword(material, "_CLEARCOAT", false);
                    CoreUtils.SetKeyword(material, "_CLEARCOATMAP", true);
                }
                else
                {
                    CoreUtils.SetKeyword(material, "_CLEARCOAT", true);
                    CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false);
                }
            }
            else
            {
                CoreUtils.SetKeyword(material, "_CLEARCOAT", false);
                CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false);
            }
        }
    }
}
