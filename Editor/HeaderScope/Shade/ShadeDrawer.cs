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
            materialEditor.TexturePropertySingleLine(ShadeStyles.FirstShadeMap, PropContainer.FirstShadeMap, PropContainer.FirstShadeColor);
            materialEditor.ShaderProperty(PropContainer.FirstShadeBorderPos, ShadeStyles.FirstShadeBorderPos);
            materialEditor.ShaderProperty(PropContainer.FirstShadeBorderBlur, ShadeStyles.FirstShadeBorderBlur);
        }
    }
}
