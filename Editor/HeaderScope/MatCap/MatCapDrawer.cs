using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class MatCapDrawer : HeaderScopeDrawerBase<MatCapPropertyContainer>
    {
        public MatCapDrawer(MatCapPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useMarCap = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMatCap, MatCapStyles.UseMatCap);
            if (useMarCap)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    materialEditor.TexturePropertySingleLine(MatCapStyles.MatCapMap, PropContainer.MatCapMap, PropContainer.MatCapColor);
                    materialEditor.TextureScaleOffsetProperty(PropContainer.MatCapMap);
                    materialEditor.ShaderProperty(PropContainer.MatCapMainLightEffectiveness, MatCapStyles.MatCapMainLightEffectiveness);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapCorrectPerspectiveDistortion, MatCapStyles.MatCapCorrectPerspectiveDistortion);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.MatCapStabilizeCameraZRotation, MatCapStyles.MatCapStabilizeCameraZRotation);
                }
            }
        }
    }
}
