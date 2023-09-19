using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class LightDrawer : HeaderScopeDrawerBase<LightPropertyContainer>
    {
        public LightDrawer(LightPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            EditorGUILayout.LabelField("Main Light", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope(1))
            {
                materialEditor.ShaderProperty(PropContainer.MainLightColorWeight, LightStyles.MainLightColorWeight);
                // TODO: alignがなんかズレてるので自前実装してちゃんと直す
                EditorGUILayout.BeginHorizontal();
                bool useMainLightUpperLimit =
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMainLightUpperLimit,
                        LightStyles.MainLightUpperLimit);
                using (new EditorGUI.DisabledScope(!useMainLightUpperLimit))
                    materialEditor.ShaderProperty(PropContainer.MainLightUpperLimit, string.Empty);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                bool useMainLightLowerLimit =
                    HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMainLightLowerLimit,
                        LightStyles.MainLightLowerLimit);
                using (new EditorGUI.DisabledScope(!useMainLightLowerLimit))
                    materialEditor.ShaderProperty(PropContainer.MainLightLowerLimit, string.Empty);
                EditorGUILayout.EndHorizontal();
            }

            HumToonGUIUtils.Space();

            EditorGUILayout.LabelField("Additional Lights", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                materialEditor.ShaderProperty(PropContainer.AdditionalLightsColorWeight, LightStyles.AdditionalLightsColorWeight);
            }
        }
    }
}
