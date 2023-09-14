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
                using (new EditorGUI.IndentLevelScope(1))
                {
                    materialEditor.TexturePropertySingleLine(MatCapStyles.MatCapMap, PropContainer.MatCapMap, PropContainer.MatCapColor);
                }
            }
        }
    }
}
