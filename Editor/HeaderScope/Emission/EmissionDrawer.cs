using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class EmissionDrawer : HeaderScopeDrawerBase<EmissionPropertiesContainer>
    {
        public EmissionDrawer(EmissionPropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useFirstEmission = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseEmission, EmissionStyles.UseEmission);
            if (useFirstEmission)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    materialEditor.TexturePropertySingleLine(EmissionStyles.EmissionMap, PropContainer.EmissionMap, PropContainer.EmissionColor);
                    materialEditor.ShaderProperty(PropContainer.EmissionIntensity, EmissionStyles.EmissionIntensity);
                    materialEditor.ShaderProperty(PropContainer.EmissionFactorR, EmissionStyles.EmissionFactorR);
                    materialEditor.ShaderProperty(PropContainer.EmissionFactorG, EmissionStyles.EmissionFactorG);
                    materialEditor.ShaderProperty(PropContainer.EmissionFactorB, EmissionStyles.EmissionFactorB);
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.OverrideEmissionColor, EmissionStyles.OverrideEmissionColor);
                }
            }
        }
    }
}
