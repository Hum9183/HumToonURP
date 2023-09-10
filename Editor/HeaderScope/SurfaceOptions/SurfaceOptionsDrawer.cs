using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public class SurfaceOptionsDrawer : HeaderScopeDrawerBase<SurfaceOptionsPropertyContainer>
    {
        public SurfaceOptionsDrawer(SurfaceOptionsPropertyContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            EditorGUIUtility.labelWidth = 0f;

            HumToonGUIUtils.DoPopup<SurfaceType>(materialEditor, PropContainer.SurfaceType, SurfaceOptionsStyles.SurfaceType);

            if ((SurfaceType)PropContainer.SurfaceType.floatValue is SurfaceType.Transparent)
            {
                HumToonGUIUtils.DoPopup<TransparentBlendMode>(materialEditor, PropContainer.BlendMode, SurfaceOptionsStyles.BlendingMode);
            }

            HumToonGUIUtils.DoPopup<RenderFace>(materialEditor, PropContainer.CullMode, SurfaceOptionsStyles.RenderFace); // NOTE: RenderFaceで表裏の差を吸収

            HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.AlphaClip, SurfaceOptionsStyles.AlphaClip);

            if (PropContainer.AlphaClip.floatValue is 1)
                materialEditor.ShaderProperty(PropContainer.Cutoff, SurfaceOptionsStyles.Cutoff, 1);

            HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.ReceiveShadows, SurfaceOptionsStyles.ReceiveShadow);
        }
    }
}
