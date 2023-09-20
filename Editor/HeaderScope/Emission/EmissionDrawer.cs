using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class EmissionDrawer : HeaderScopeDrawerBase<EmissionPropertyContainer>
    {
        public EmissionDrawer(EmissionPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
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
                }

            }
        }
    }
}
