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
            DrawNormalArea(materialEditor);
            DrawTileOffset(materialEditor);
        }

        private void DrawBaseMap(MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(SurfaceInputsStyles.BaseMap, PropContainer.BaseMap, PropContainer.BaseColor);
        }

        private void DrawNormalArea(MaterialEditor materialEditor)
        {
            var normalMap = PropContainer.BumpMap;
            var normalScale = PropContainer.BumpScale;

            if (normalScale is not null)
            {
                materialEditor.TexturePropertySingleLine(SurfaceInputsStyles.NormalMap, normalMap,
                    normalMap.textureValue ? normalScale : null);
                DrawMobileOptions();
            }
            else
            {
                materialEditor.TexturePropertySingleLine(SurfaceInputsStyles.NormalMap, normalMap);
            }

            return;

            void DrawMobileOptions()
            {
                if (normalScale.floatValue is not 1
                    && UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget))
                    if (materialEditor.HelpBoxWithButton(SurfaceInputsStyles.BumpScaleNotSupported, SurfaceInputsStyles.FixNormalNow))
                        normalScale.floatValue = 1;
            }
        }

        private void DrawTileOffset(MaterialEditor materialEditor)
        {
            materialEditor.TextureScaleOffsetProperty(PropContainer.BaseMap);
        }
    }
}
