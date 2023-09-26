using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    public partial class HumToonInspector
    {
        /// <summary>
        /// Draws the surface options GUI.
        /// </summary>
        private void DrawSurfaceInputs(Material material)
        {
            DrawBaseMap();
            DoMetallicSpecularArea(material);
            DrawNormalArea();
            DoHeightmapArea();
            DrawOcclusionMap();
            DrawEmissionProperties(material);
            DrawTileOffset();
        }

        private void DrawBaseMap()
        {
            _materialEditor.TexturePropertySingleLine(HumToonStyles.BaseMap, _matPropContainer.BaseMap, _matPropContainer.BaseColor);
        }

        private void DoMetallicSpecularArea(Material material)
        {
            string[] smoothnessChannelNames;
            bool hasGlossMap = false;
            if ((WorkflowMode)_litMatPropContainer.WorkflowMode.floatValue is WorkflowMode.Metallic)
            {
                hasGlossMap = _litMatPropContainer.MetallicGlossMap.textureValue != null;
                smoothnessChannelNames = LitStyles.MetallicSmoothnessChannelNames;
                _materialEditor.TexturePropertySingleLine(LitStyles.MetallicMap, _litMatPropContainer.MetallicGlossMap,
                    hasGlossMap ? null : _litMatPropContainer.Metallic);
            }
            else
            {
                hasGlossMap = _litMatPropContainer.SpecGlossMap.textureValue != null;
                smoothnessChannelNames = LitStyles.SpecularSmoothnessChannelNames;
                HumToonGUIUtils.TextureColorProps(_materialEditor, LitStyles.SpecularMap, _litMatPropContainer.SpecGlossMap,
                    hasGlossMap ? null : _litMatPropContainer.SpecColor);
            }

            DoSmoothness();

            void DoSmoothness()
            {
                EditorGUI.indentLevel += 2;

                _materialEditor.ShaderProperty(_litMatPropContainer.Smoothness, LitStyles.Smoothness);

                var smoothnessTextureChannel = _litMatPropContainer.SmoothnessTextureChannel;
                EditorGUI.indentLevel++;
                EditorGUI.showMixedValue = smoothnessTextureChannel.hasMixedValue;
                if (Utils.IsOpaque(material))
                {
                    HumToonGUIUtils.DoPopup(_materialEditor, smoothnessTextureChannel, LitStyles.SmoothnessTextureChannel, smoothnessChannelNames);
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.Popup(LitStyles.SmoothnessTextureChannel, 0, smoothnessChannelNames);
                    }
                }
                EditorGUI.showMixedValue = false;
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel -= 2;
            }
        }

        private void DrawNormalArea()
        {
            var normalMap = _litMatPropContainer.BumpMap;
            var normalScale = _litMatPropContainer.BumpScale;

            if (normalScale is not null)
            {
                _materialEditor.TexturePropertySingleLine(HumToonStyles.NormalMap, normalMap,
                    normalMap.textureValue ? normalScale : null);
                DrawMobileOptions();
            }
            else
            {
                _materialEditor.TexturePropertySingleLine(HumToonStyles.NormalMap, normalMap);
            }

            void DrawMobileOptions()
            {
                if (normalScale.floatValue is not 1
                    && UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget))
                    if (_materialEditor.HelpBoxWithButton(HumToonStyles.BumpScaleNotSupported, HumToonStyles.FixNormalNow))
                        normalScale.floatValue = 1;
            }
        }

        private void DoHeightmapArea()
        {
            _materialEditor.TexturePropertySingleLine(LitStyles.HeightMap, _litMatPropContainer.ParallaxMap,
                _litMatPropContainer.ParallaxMap.textureValue ? _litMatPropContainer.Parallax : null);
        }

        private void DrawOcclusionMap()
        {
            _materialEditor.TexturePropertySingleLine(LitStyles.Occlusion, _litMatPropContainer.OcclusionMap,
                _litMatPropContainer.OcclusionMap.textureValue ? _litMatPropContainer.OcclusionStrength : null);
        }

        private void DrawEmissionProperties(Material material)
        {
            bool emissive = _materialEditor.EmissionEnabledProperty();
            using (new EditorGUI.DisabledScope(!emissive))
            {
                using (new EditorGUI.IndentLevelScope(2))
                {
                    _materialEditor.TexturePropertyWithHDRColor(HumToonStyles.EmissionMap, _matPropContainer.EmissionMap, _matPropContainer.EmissionColor, false);
                }
            }

            // If texture was assigned and color was black set color to white.
            var hadEmissionTexture = _matPropContainer.EmissionMap?.textureValue is not null;
            var brightness = _matPropContainer.EmissionColor.colorValue.maxColorComponent;
            if (hadEmissionTexture && brightness <= 0f)
                _matPropContainer.EmissionColor.colorValue = Color.white;

            if (emissive)
            {
                // Change the GI emission flag and fix it up with emissive as black if necessary.
                _materialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
            }
        }

        private void DrawTileOffset()
        {
            _materialEditor.TextureScaleOffsetProperty(_matPropContainer.BaseMap);
        }
    }
}
