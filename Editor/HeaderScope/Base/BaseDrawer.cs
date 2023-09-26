using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScope.Base
{
    public class BaseDrawer : HeaderScopeDrawerBase<BasePropertiesContainer>
    {
        public BaseDrawer(BasePropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            DrawBaseMap(materialEditor);
            DrawTileOffset(materialEditor);
        }

        private void DrawBaseMap(MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(BaseStyles.BaseMap, PropContainer.BaseMap, PropContainer.BaseColor);
        }

        private void DrawTileOffset(MaterialEditor materialEditor)
        {
            materialEditor.TextureScaleOffsetProperty(PropContainer.BaseMap);
        }
    }
}
