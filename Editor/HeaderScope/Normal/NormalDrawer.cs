using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class NormalDrawer : HeaderScopeDrawerBase<NormalPropertyContainer>
    {
        public NormalDrawer(NormalPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            DrawNormalArea(materialEditor);
        }

        private void DrawNormalArea(MaterialEditor materialEditor)
        {
            var normalMap = PropContainer.BumpMap;
            var normalScale = PropContainer.BumpScale;

            if (normalScale is not null)
            {
                materialEditor.TexturePropertySingleLine(NormalStyles.NormalMap, normalMap,
                    normalMap.textureValue ? normalScale : null);
                DrawMobileOptions();
            }
            else
            {
                materialEditor.TexturePropertySingleLine(NormalStyles.NormalMap, normalMap);
            }

            return;

            void DrawMobileOptions()
            {
                if (normalScale.floatValue is not 1
                    && UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget))
                    if (materialEditor.HelpBoxWithButton(NormalStyles.BumpScaleNotSupported, NormalStyles.FixNormalNow))
                        normalScale.floatValue = 1;
            }
        }
    }
}
