using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes.Normal
{
    public class NormalDrawer : HeaderScopeDrawerBase<NormalPropertiesContainer>
    {
        public NormalDrawer(NormalPropertiesContainer propContainer, Func<GUIContent> headerStyleFunc, uint expandable)
            : base(propContainer, headerStyleFunc, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            var normalMap = PropContainer.BumpMap;
            var normalScale = PropContainer.BumpScale;

            HumToonGUIUtils.TextureAndRangePropertiesSingleLine(materialEditor, normalMap, normalScale, NormalStyles.NormalMap);
            DrawMobileOptions();

            return;

            void DrawMobileOptions()
            {
                if (normalScale.floatValue.IsOne() is false
                    && UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget))
                    if (materialEditor.HelpBoxWithButton(NormalStyles.BumpScaleNotSupported, NormalStyles.FixNormalNow))
                        normalScale.floatValue = 1;
            }
        }
    }
}
