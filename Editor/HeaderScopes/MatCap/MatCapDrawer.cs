using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public class MatCapDrawer : HeaderScopeDrawerBase<MatCapPropertiesContainer>
    {
        public MatCapDrawer(MatCapPropertiesContainer propContainer, Func<GUIContent> headerStyleFunc, uint expandable)
            : base(propContainer, headerStyleFunc, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useMarCap = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMatCap, MatCapStyles.UseMatCap);
            if (useMarCap)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    DrawMatCapMap(materialEditor);
                    materialEditor.ShaderProperty(PropContainer.MatCapIntensity, MatCapStyles.MatCapIntensity);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapCorrectPerspectiveDistortion, MatCapStyles.MatCapCorrectPerspectiveDistortion);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapStabilizeCameraZRotation, MatCapStyles.MatCapStabilizeCameraZRotation);
                    HumToonGUIUtils.TextureAndRangePropertiesSingleLine(materialEditor, PropContainer.MatCapMask, PropContainer.MatCapMaskIntensity, MatCapStyles.MatCapMask);
                    materialEditor.ShaderProperty(PropContainer.MatCapMainLightEffectiveness, MatCapStyles.MatCapMainLightEffectiveness);
                }
            }
        }

        private void DrawMatCapMap(MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(MatCapStyles.MatCapMap, PropContainer.MatCapMap, PropContainer.MatCapColor);
            using (new EditorGUI.IndentLevelScope())
            {
                materialEditor.TextureScaleOffsetProperty(PropContainer.MatCapMap);
                materialEditor.ShaderProperty(PropContainer.MatCapMapMipLevel, MatCapStyles.MatCapMapMipLevel);
            }
        }
    }
}
