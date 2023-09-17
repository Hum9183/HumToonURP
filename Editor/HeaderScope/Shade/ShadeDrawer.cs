using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class ShadeDrawer : HeaderScopeDrawerBase<ShadePropertyContainer>
    {
        public ShadeDrawer(ShadePropertyContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useFirstShade = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseFirstShade, ShadeStyles.UseFirstShade);
            if (useFirstShade)
            {
                using (new EditorGUI.IndentLevelScope(1))
                {
                    materialEditor.TexturePropertySingleLine(ShadeStyles.FirstShadeMap, PropContainer.FirstShadeMap, PropContainer.FirstShadeColor);
                    materialEditor.ShaderProperty(PropContainer.FirstShadeBorderPos, ShadeStyles.FirstShadeBorderPos);
                    materialEditor.ShaderProperty(PropContainer.FirstShadeBorderBlur, ShadeStyles.FirstShadeBorderBlur);
                }
                HumToonGUIUtils.Space();
            }


            bool useSecondShade = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseSecondShade, ShadeStyles.UseSecondShade);
            if (useSecondShade)
            {
                using (new EditorGUI.IndentLevelScope(1))
                {
                    materialEditor.TexturePropertySingleLine(ShadeStyles.SecondShadeMap, PropContainer.SecondShadeMap, PropContainer.SecondShadeColor);
                    materialEditor.ShaderProperty(PropContainer.SecondShadeBorderPos, ShadeStyles.SecondShadeBorderPos);
                    materialEditor.ShaderProperty(PropContainer.SecondShadeBorderBlur, ShadeStyles.SecondShadeBorderBlur);
                }
            }
        }
    }
}
