using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class SurfaceInputsDrawer : HeaderScopeDrawerBase<SurfaceInputsPropertyContainer>
    {
        public SurfaceInputsDrawer(SurfaceInputsPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
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
            materialEditor.TexturePropertySingleLine(SurfaceInputsStyles.BaseMap, PropContainer.BaseMap, PropContainer.BaseColor);
        }

        private void DrawTileOffset(MaterialEditor materialEditor)
        {
            materialEditor.TextureScaleOffsetProperty(PropContainer.BaseMap);
        }
    }
}
