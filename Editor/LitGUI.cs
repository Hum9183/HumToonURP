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
        /// <summary>
        /// Draws the surface inputs GUI.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="materialEditor"></param>
        /// <param name="material"></param>
        public static void Inputs(LitMaterialPropertyContainer properties, MaterialEditor materialEditor, Material material)
        {
            DoMetallicSpecularArea(properties, materialEditor, material);
            HumToonInspector.DrawNormalArea(materialEditor, properties.BumpMap, properties.BumpScale);

            if (HeightmapAvailable(material))
                DoHeightmapArea(properties, materialEditor);

            if (properties.OcclusionMap != null)
            {
                materialEditor.TexturePropertySingleLine(LitStyles.Occlusion, properties.OcclusionMap,
                    properties.OcclusionMap.textureValue != null ? properties.OcclusionStrength : null);
            }

            // Check that we have all the required properties for clear coat,
            // otherwise we will get null ref exception from MaterialEditor GUI helpers.
            if (ClearCoatAvailable(material))
                DoClearCoat(properties, materialEditor, material);
        }

        private static bool ClearCoatAvailable(Material material)
        {
            return material.HasProperty("_ClearCoat")
                && material.HasProperty("_ClearCoatMap")
                && material.HasProperty("_ClearCoatMask")
                && material.HasProperty("_ClearCoatSmoothness");
        }

        private static bool HeightmapAvailable(Material material)
        {
            return material.HasProperty("_Parallax")
                && material.HasProperty("_ParallaxMap");
        }

        private static void DoHeightmapArea(LitMaterialPropertyContainer properties, MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(LitStyles.HeightMap, properties.ParallaxMap,
                properties.ParallaxMap.textureValue != null ? properties.ParallaxScale : null);
        }

        private static bool ClearCoatEnabled(Material material)
        {
            return material.HasProperty("_ClearCoat") && material.GetFloat("_ClearCoat") > 0.0;
        }

        /// <summary>
        /// Draws the clear coat GUI.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="materialEditor"></param>
        /// <param name="material"></param>
        public static void DoClearCoat(LitMaterialPropertyContainer properties, MaterialEditor materialEditor, Material material)
        {
            materialEditor.ShaderProperty(properties.ClearCoat, LitStyles.ClearCoat);
            var coatEnabled = material.GetFloat("_ClearCoat") > 0.0;

            EditorGUI.BeginDisabledGroup(!coatEnabled);
            {
                EditorGUI.indentLevel += 2;
                materialEditor.TexturePropertySingleLine(LitStyles.ClearCoatMask, properties.ClearCoatMap, properties.ClearCoatMask);

                // Texture and HDR color controls
                materialEditor.ShaderProperty(properties.ClearCoatSmoothness, LitStyles.ClearCoatSmoothness);

                EditorGUI.indentLevel -= 2;
            }
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Draws the metallic/specular area GUI.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="materialEditor"></param>
        /// <param name="material"></param>
        public static void DoMetallicSpecularArea(LitMaterialPropertyContainer properties, MaterialEditor materialEditor, Material material)
        {
            string[] smoothnessChannelNames;
            bool hasGlossMap = false;
            if (properties.WorkflowMode == null ||
                (WorkflowMode)properties.WorkflowMode.floatValue == WorkflowMode.Metallic)
            {
                Debug.Log("loop");
                Debug.Log(properties.MetallicGlossMap);
                hasGlossMap = properties.MetallicGlossMap.textureValue != null;
                smoothnessChannelNames = LitStyles.MetallicSmoothnessChannelNames;
                materialEditor.TexturePropertySingleLine(LitStyles.MetallicMap, properties.MetallicGlossMap,
                    hasGlossMap ? null : properties.Metallic);
            }
            else
            {
                hasGlossMap = properties.SpecGlossMap.textureValue != null;
                smoothnessChannelNames = LitStyles.SpecularSmoothnessChannelNames;
                HumToonInspector.TextureColorProps(materialEditor, LitStyles.SpecularMap, properties.SpecGlossMap,
                    hasGlossMap ? null : properties.SpecColor);
            }
            DoSmoothness(materialEditor, material, properties.Smoothness, properties.SmoothnessTextureChannel, smoothnessChannelNames);
        }

        /// <summary>
        /// Draws the smoothness GUI.
        /// </summary>
        /// <param name="materialEditor"></param>
        /// <param name="material"></param>
        /// <param name="smoothness"></param>
        /// <param name="smoothnessMapChannel"></param>
        /// <param name="smoothnessChannelNames"></param>
        public static void DoSmoothness(MaterialEditor materialEditor, Material material, MaterialProperty smoothness, MaterialProperty smoothnessMapChannel, string[] smoothnessChannelNames)
        {
            EditorGUI.indentLevel += 2;

            materialEditor.ShaderProperty(smoothness, LitStyles.Smoothness);

            if (smoothnessMapChannel != null) // smoothness channel
            {
                var opaque = HumToonInspector.IsOpaque(material);
                EditorGUI.indentLevel++;
                EditorGUI.showMixedValue = smoothnessMapChannel.hasMixedValue;
                if (opaque)
                {
                    MaterialEditor.BeginProperty(smoothnessMapChannel);
                    EditorGUI.BeginChangeCheck();
                    var smoothnessSource = (int)smoothnessMapChannel.floatValue;
                    smoothnessSource = EditorGUILayout.Popup(LitStyles.SmoothnessTextureChannel, smoothnessSource, smoothnessChannelNames);
                    if (EditorGUI.EndChangeCheck())
                        smoothnessMapChannel.floatValue = smoothnessSource;
                    MaterialEditor.EndProperty();
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Popup(LitStyles.SmoothnessTextureChannel, 0, smoothnessChannelNames);
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.showMixedValue = false;
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel -= 2;
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
