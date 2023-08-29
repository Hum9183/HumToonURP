using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        /// <summary>
        /// Draws the surface options GUI.
        /// </summary>
        private void DrawSurfaceOptions(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;

            HumToonGUIUtils.DoPopup<WorkflowMode>(_materialEditor, _litMatPropContainer.WorkflowMode, LitStyles.WorkflowMode);
            HumToonGUIUtils.DoPopup<SurfaceType>(_materialEditor, _matPropContainer.SurfaceType, HumToonStyles.SurfaceType);

            if ((SurfaceType)_matPropContainer.SurfaceType.floatValue is SurfaceType.Transparent)
            {
                HumToonGUIUtils.DoPopup<TransparentBlendMode>(_materialEditor, _matPropContainer.BlendMode, HumToonStyles.BlendingMode);

                TransparentBlendMode transparentBlendMode = (TransparentBlendMode)material.GetFloat(HumToonPropertyNames.BlendMode);
                bool isEnabled = transparentBlendMode is TransparentBlendMode.Alpha || transparentBlendMode is TransparentBlendMode.Additive;
                if (isEnabled)
                    HumToonGUIUtils.DrawFloatToggleProperty(_matPropContainer.BlendModePreserveSpecular, HumToonStyles.BlendModePreserveSpecular, 1);
            }

            HumToonGUIUtils.DoPopup<RenderFace>(_materialEditor, _matPropContainer.CullMode, HumToonStyles.RenderFace); // NOTE: RenderFaceで表裏の差を吸収

            HumToonGUIUtils.DrawFloatToggleProperty(_matPropContainer.AlphaClip, HumToonStyles.AlphaClip);

            if (_matPropContainer.AlphaClip.floatValue is 1)
                _materialEditor.ShaderProperty(_matPropContainer.Cutoff, HumToonStyles.Cutoff, 1);

            HumToonGUIUtils.DrawFloatToggleProperty(_matPropContainer.ReceiveShadows, HumToonStyles.ReceiveShadow);
        }
    }
}
