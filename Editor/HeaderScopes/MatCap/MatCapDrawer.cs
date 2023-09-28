using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public class MatCapDrawer : HeaderScopeDrawerBase<MatCapPropertiesContainer>
    {
        public MatCapDrawer(MatCapPropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            // TODO: Toggleの真横にIntensityのSliderがあるほうが良いかもしれない
            bool useMarCap = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMatCap, MatCapStyles.UseMatCap);
            if (useMarCap)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    materialEditor.TexturePropertySingleLine(MatCapStyles.MatCapMap, PropContainer.MatCapMap, PropContainer.MatCapColor);
                    materialEditor.TextureScaleOffsetProperty(PropContainer.MatCapMap);
                    materialEditor.ShaderProperty(PropContainer.MatCapIntensity, MatCapStyles.MatCapIntensity);
                    materialEditor.ShaderProperty(PropContainer.MatCapMapMipLevel, MatCapStyles.MatCapMapMipLevel);
                    materialEditor.ShaderProperty(PropContainer.MatCapMainLightEffectiveness, MatCapStyles.MatCapMainLightEffectiveness);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapCorrectPerspectiveDistortion, MatCapStyles.MatCapCorrectPerspectiveDistortion);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapStabilizeCameraZRotation, MatCapStyles.MatCapStabilizeCameraZRotation);
                }
            }
        }
    }
}
