using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor.LitBased
{
    public class MaterialKeywordsSetter
    {
        /// <summary>
        /// Called when a material has been changed.
        /// </summary>
        public static void Set(Material material, bool litDetail = false)
        {
            SetMaterialKeywords(material);
            SetMaterialKeywordsLit(material);
            if (litDetail)
                SetMaterialKeywordsLitDetail(material);
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

        private static void SetMaterialKeywordsLit(Material material)
        {
            SetupSpecularWorkflowKeyword(out bool isSpecularWorkFlow);

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
                var opaque = Utils.IsOpaque(material);
                CoreUtils.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                    GetSmoothnessTextureChannel() is SmoothnessTextureChannel.AlbedoAlpha && opaque);
            }

            CoreUtils.SetKeyword(material, "_CLEARCOAT", false);
            CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false);

            // (shared by all lit shaders, including shadergraph Lit Target and Lit.shader)
             void SetupSpecularWorkflowKeyword(out bool isSpecularWorkflow)
            {
                isSpecularWorkflow = false;     // default is metallic workflow
                if (material.HasProperty("_WorkflowMode"))
                    isSpecularWorkflow = ((WorkflowMode)material.GetFloat("_WorkflowMode")) == WorkflowMode.Specular;
                CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkflow);
            }

             SmoothnessTextureChannel GetSmoothnessTextureChannel()
             {
                 int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
                 if (ch == (int)SmoothnessTextureChannel.AlbedoAlpha)
                     return SmoothnessTextureChannel.AlbedoAlpha;

                 return SmoothnessTextureChannel.SpecularMetallicAlpha;
             }
        }

        private static void SetMaterialKeywordsLitDetail(Material material)
        {
            if (material.HasProperty("_DetailAlbedoMap") && material.HasProperty("_DetailNormalMap") && material.HasProperty("_DetailAlbedoMapScale"))
            {
                bool isScaled = material.GetFloat("_DetailAlbedoMapScale") != 1.0f;
                bool hasDetailMap = material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap");
                CoreUtils.SetKeyword(material, "_DETAIL_MULX2", !isScaled && hasDetailMap);
                CoreUtils.SetKeyword(material, "_DETAIL_SCALED", isScaled && hasDetailMap);
            }
        }
    }
}
