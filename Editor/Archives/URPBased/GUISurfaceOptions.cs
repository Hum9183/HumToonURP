using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Hum.HumToon.Editor.Archives.URPBased
{
    internal partial class HumToonGUI
    {
        private void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            EditorGUIUtility.labelWidth = 0f;

            DrawSurfaceType(material);
            HumShaderGUIUtils.DoPopup(_materialEditor, _matProps.CullMode, Styles.cullingText, Styles.renderFaceNames);
            // HumShaderGUIUtils.DoPopup(_materialEditor, _matProps.ZWrite, Styles.zwriteText, Styles.zWriteNames);

            DrawAlphaClip(material);

            // TODO:
            // ShaderGUIUtil.DrawFloatToggleProperty(Styles.castShadowText, _matProps.CastShadows);
            // ShaderGUIUtil.DrawFloatToggleProperty(Styles.receiveShadowText, _matProps.ReceiveShadows);
        }

        private void DrawSurfaceType(Material material)
        {
            SurfaceType surfaceType = (SurfaceType)HumShaderGUIUtils.DoPopup<SurfaceType>(_materialEditor, _matProps.SurfaceType, Styles.surfaceType);
            BlendMode blendMode = (BlendMode)HumShaderGUIUtils.DoPopup<BlendMode>(_materialEditor, _matProps.BlendMode, Styles.blendingMode);
        }

        private void DrawAlphaClip(Material material)
        {
            bool alphaClip = HumShaderGUIUtils.DrawHumToggleProperty(Styles.alphaClipText, _matProps.AlphaClip);
            if (alphaClip)
                _materialEditor.ShaderProperty(_matProps.AlphaCutoff, Styles.alphaClipThresholdText, 1);

            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);
        }
    }
}
