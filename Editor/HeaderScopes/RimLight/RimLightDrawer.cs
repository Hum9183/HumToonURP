using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes.RimLight
{
    public class RimLightDrawer : HeaderScopeDrawerBase<RimLightPropertiesContainer>
    {
        public RimLightDrawer(RimLightPropertiesContainer propContainer, Func<GUIContent> headerStyleFunc, uint expandable)
            : base(propContainer, headerStyleFunc, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            bool useRimLight = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseRimLight, RimLightStyles.UseRimLight);
            if (useRimLight)
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
