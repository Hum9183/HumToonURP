using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class LightDrawer : HeaderScopeDrawerBase<LightPropertiesContainer>
    {
        public LightDrawer(LightPropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            DrawMainLight(materialEditor);
            HumToonGUIUtils.Space();
            DrawAdditionalLights(materialEditor);
        }

        private void DrawMainLight(MaterialEditor materialEditor)
        {
            EditorGUILayout.LabelField("Main Light", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope(1))
            {
                materialEditor.ShaderProperty(PropContainer.MainLightColorWeight, LightStyles.MainLightColorWeight);
                HumToonGUIUtils.FloatToggleAndRangePropertiesSingleLine(materialEditor,PropContainer.UseMainLightUpperLimit, PropContainer.MainLightUpperLimit, LightStyles.MainLightUpperLimit);
                HumToonGUIUtils.FloatToggleAndRangePropertiesSingleLine(materialEditor,PropContainer.UseMainLightLowerLimit, PropContainer.MainLightLowerLimit, LightStyles.MainLightLowerLimit);
            }
        }

        private void DrawAdditionalLights(MaterialEditor materialEditor)
        {
            EditorGUILayout.LabelField("Additional Lights", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                materialEditor.ShaderProperty(PropContainer.AdditionalLightsColorWeight, LightStyles.AdditionalLightsColorWeight);
            }
        }
    }
}
