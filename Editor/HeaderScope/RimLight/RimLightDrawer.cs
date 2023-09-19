using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class RimLightDrawer : HeaderScopeDrawerBase<RimLightPropertyContainer>
    {
        public RimLightDrawer(RimLightPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
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
                }
            }
        }
    }
}
