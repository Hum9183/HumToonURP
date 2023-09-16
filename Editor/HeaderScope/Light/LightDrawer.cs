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
            materialEditor.ShaderProperty(PropContainer.MainLightColorWeight, LightStyles.MainLightColorWeight);

            // TODO: BeginHorizontal()のUsingを自作しても良いかもしれない
            EditorGUILayout.BeginHorizontal(); // TODO: alignがなんかズレてるのでちゃんと直す
            bool useMainLightUpperLimit = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMainLightUpperLimit, LightStyles.MainLightUpperLimit);
            using (new EditorGUI.DisabledScope(!useMainLightUpperLimit))
                materialEditor.ShaderProperty(PropContainer.MainLightUpperLimit, string.Empty);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            bool useMainLightLowerLimit = HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.UseMainLightLowerLimit, LightStyles.MainLightLowerLimit);
            using (new EditorGUI.DisabledScope(!useMainLightLowerLimit))
                materialEditor.ShaderProperty(PropContainer.MainLightLowerLimit, string.Empty);
            EditorGUILayout.EndHorizontal();

            materialEditor.ShaderProperty(PropContainer.AdditionalLightsColorWeight, LightStyles.AdditionalLightsColorWeight);
        }
    }
}