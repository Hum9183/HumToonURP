using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    public partial class HumToonInspector
    {
        private void DrawDetailInputs(Material material)
        {
            _materialEditor.TexturePropertySingleLine(LitDetailStyles.detailMaskText, _litDetailMatPropContainer.DetailMask);
            _materialEditor.TexturePropertySingleLine(LitDetailStyles.detailAlbedoMapText, _litDetailMatPropContainer.DetailAlbedoMap,
                _litDetailMatPropContainer.DetailAlbedoMap.textureValue != null ? _litDetailMatPropContainer.DetailAlbedoMapScale : null);
            if (_litDetailMatPropContainer.DetailAlbedoMapScale.floatValue is not 1.0f)
            {
                EditorGUILayout.HelpBox(LitDetailStyles.detailAlbedoMapScaleInfo.text, MessageType.Info, true);
            }
            var detailAlbedoTexture = _litDetailMatPropContainer.DetailAlbedoMap.textureValue as Texture2D;
            if (detailAlbedoTexture != null && GraphicsFormatUtility.IsSRGBFormat(detailAlbedoTexture.graphicsFormat))
            {
                EditorGUILayout.HelpBox(LitDetailStyles.detailAlbedoMapFormatError.text, MessageType.Warning, true);
            }
            _materialEditor.TexturePropertySingleLine(LitDetailStyles.detailNormalMapText, _litDetailMatPropContainer.DetailNormalMap,
                _litDetailMatPropContainer.DetailNormalMap.textureValue != null ? _litDetailMatPropContainer.DetailNormalMapScale : null);
            _materialEditor.TextureScaleOffsetProperty(_litDetailMatPropContainer.DetailAlbedoMap);
        }

    }
}
