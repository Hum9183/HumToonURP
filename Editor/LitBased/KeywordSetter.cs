using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor.LitBased
{
    public static class KeywordSetter
    {
        public static void Set(Material material, bool isOpaque, bool alphaClip,
            bool transparentPreserveSpecular, bool transparentAlphaModulate)
        {
            // Receive Shadows
            bool receiveShadows = material.GetFloat(HumToonPropertyNames.ReceiveShadows).ToBool();
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, receiveShadows is false);

            // Alpha test
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);

            // Transparent
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT, isOpaque is false);

            // PreserveSpecular(Transparent)
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHAPREMULTIPLY_ON, transparentPreserveSpecular);

            // AlphaModulate(Transparent)
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHAMODULATE_ON, transparentAlphaModulate);

            // Normal Map
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, material.GetTexture("_BumpMap"));

            // others
            CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF", material.GetFloat("_SpecularHighlights").ToBool() is false);
            CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF", material.GetFloat("_EnvironmentReflections").ToBool() is false);
            CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
            CoreUtils.SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));

            CoreUtils.SetKeyword(material, "_CLEARCOAT", false);    // TODO: 不要かも
            CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false); // TODO: 不要かも

            // Detail
            bool isScaled = material.GetFloat("_DetailAlbedoMapScale").IsOne() is false;
            bool hasDetailMap = material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap");
            CoreUtils.SetKeyword(material, "_DETAIL_MULX2", !isScaled && hasDetailMap);
            CoreUtils.SetKeyword(material, "_DETAIL_SCALED", isScaled && hasDetailMap);

            // workflow
            SetupSpecularWorkflowKeyword(material, out bool isSpecularWorkflow);
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            var specularGlossMap = isSpecularWorkflow ? "_SpecGlossMap" : "_MetallicGlossMap";
            var hasGlossMap = material.GetTexture(specularGlossMap) != null;
            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);
        }

        private static void SetupSpecularWorkflowKeyword(Material material, out bool isSpecularWorkflow)
        {
            isSpecularWorkflow = false;     // default is metallic workflow
            if (material.HasProperty("_WorkflowMode"))
                isSpecularWorkflow = ((WorkflowMode)material.GetFloat("_WorkflowMode")) == WorkflowMode.Specular;
            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkflow);
        }

        static SmoothnessTextureChannel GetSmoothnessTextureChannel(Material material)
        {
            int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int)SmoothnessTextureChannel.AlbedoAlpha)
                return SmoothnessTextureChannel.AlbedoAlpha;

            return SmoothnessTextureChannel.SpecularMetallicAlpha;
        }
    }
}
