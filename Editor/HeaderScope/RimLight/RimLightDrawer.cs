using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class RimLightDrawer : HeaderScopeDrawerBase<RimLightPropertiesContainer>
    {
        public RimLightDrawer(RimLightPropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useMarCap = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseRimLight, RimLightStyles.UseRimLight);
            if (useMarCap)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    materialEditor.TexturePropertySingleLine(RimLightStyles.RimLightMap, PropContainer.RimLightMap, PropContainer.RimLightColor);
                    materialEditor.ShaderProperty(PropContainer.RimLightIntensity, RimLightStyles.RimLightIntensity);
                    materialEditor.ShaderProperty(PropContainer.RimLightBorderPos, RimLightStyles.RimLightBorderPos);
                    materialEditor.ShaderProperty(PropContainer.RimLightBorderBlur, RimLightStyles.RimLightBorderBlur);
                    materialEditor.ShaderProperty(PropContainer.RimLightMainLightEffectiveness, RimLightStyles.RimLightMainLightEffectiveness);
                }
            }
        }
    }
}
