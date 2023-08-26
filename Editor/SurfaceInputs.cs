using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        /// <summary>
        /// Draws the surface options GUI.
        /// </summary>
        private void DrawSurfaceInputs(Material material)
        {
            _materialEditor.TexturePropertySingleLine(HumToonStyles.BaseMap, _matPropContainer.BaseMap, _matPropContainer.BaseColor);

            DoMetallicSpecularArea(_litMatPropContainer, _materialEditor, material);
            HumToonInspector.DrawNormalArea(_materialEditor, _litMatPropContainer.BumpMap, _litMatPropContainer.BumpScale);

            if (HeightmapAvailable(material))
                DoHeightmapArea(_litMatPropContainer, _materialEditor);

            if (_litMatPropContainer.OcclusionMap != null)
            {
                _materialEditor.TexturePropertySingleLine(LitStyles.Occlusion, _litMatPropContainer.OcclusionMap,
                    _litMatPropContainer.OcclusionMap.textureValue != null ? _litMatPropContainer.OcclusionStrength : null);
            }

            // Check that we have all the required properties for clear coat,
            // otherwise we will get null ref exception from MaterialEditor GUI helpers.
            if (ClearCoatAvailable(material))
                DoClearCoat(_litMatPropContainer, _materialEditor, material);

            DrawEmissionProperties(material, true);
            DrawTileOffset(_matPropContainer.BaseMap);

        }

        #region Lit

        public static void DoMetallicSpecularArea(LitMaterialPropertyContainer properties, MaterialEditor materialEditor, Material material)
        {
            string[] smoothnessChannelNames;
            bool hasGlossMap = false;
            if (properties.WorkflowMode == null ||
                (WorkflowMode)properties.WorkflowMode.floatValue == WorkflowMode.Metallic)
            {
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

        public static void DoSmoothness(MaterialEditor materialEditor, Material material, MaterialProperty smoothness, MaterialProperty smoothnessTextureChannel, string[] smoothnessChannelNames)
        {
            EditorGUI.indentLevel += 2;

            materialEditor.ShaderProperty(smoothness, LitStyles.Smoothness);

            if (smoothnessTextureChannel != null)
            {
                var opaque = HumToonInspector.IsOpaque(material);
                EditorGUI.indentLevel++;
                EditorGUI.showMixedValue = smoothnessTextureChannel.hasMixedValue;
                if (opaque)
                {
                    MaterialEditor.BeginProperty(smoothnessTextureChannel);
                    EditorGUI.BeginChangeCheck();
                    var smoothnessSource = (int)smoothnessTextureChannel.floatValue;
                    smoothnessSource = EditorGUILayout.Popup(LitStyles.SmoothnessTextureChannel, smoothnessSource, smoothnessChannelNames);
                    if (EditorGUI.EndChangeCheck())
                        smoothnessTextureChannel.floatValue = smoothnessSource;
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

        private static bool ClearCoatAvailable(Material material)
        {
            return material.HasProperty("_ClearCoat")
                   && material.HasProperty("_ClearCoatMap")
                   && material.HasProperty("_ClearCoatMask")
                   && material.HasProperty("_ClearCoatSmoothness");
        }

        #endregion

        private void DrawEmissionProperties(Material material, bool keyword)
        {
            var emissive = true;

            if (!keyword)
            {
                DrawEmissionTextureProperty();
            }
            else
            {
                emissive = _materialEditor.EmissionEnabledProperty();
                using (new EditorGUI.DisabledScope(!emissive))
                {
                    DrawEmissionTextureProperty();
                }
            }

            // If texture was assigned and color was black set color to white
            if ((_matPropContainer.EmissionMap != null) && (_matPropContainer.EmissionColor != null))
            {
                var hadEmissionTexture = _matPropContainer.EmissionMap?.textureValue != null;
                var brightness = _matPropContainer.EmissionColor.colorValue.maxColorComponent;
                if (_matPropContainer.EmissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                    _matPropContainer.EmissionColor.colorValue = Color.white;
            }

            if (emissive)
            {
                // Change the GI emission flag and fix it up with emissive as black if necessary.
                _materialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
            }
        }

        private void DrawTileOffset(MaterialProperty textureProp)
        {
            if (textureProp != null)
                _materialEditor.TextureScaleOffsetProperty(textureProp);
        }
    }
}
