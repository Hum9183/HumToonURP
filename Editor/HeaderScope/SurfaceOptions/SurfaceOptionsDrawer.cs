using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScope.SurfaceOptions
{
    public class SurfaceOptionsDrawer : HeaderScopeDrawerBase<SurfaceOptionsPropertiesContainer>
    {
        public SurfaceOptionsDrawer(SurfaceOptionsPropertiesContainer propContainer, GUIContent headerStyle, uint expandable)
            : base(propContainer, headerStyle, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            SurfaceType surfaceType = HumToonGUIUtils.DoPopup<SurfaceType>(materialEditor, PropContainer.SurfaceType, SurfaceOptionsStyles.SurfaceType);

            if (surfaceType is SurfaceType.Transparent)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    HumToonGUIUtils.DoPopup<TransparentBlendMode>(materialEditor, PropContainer.BlendMode, SurfaceOptionsStyles.TransparentBlendMode);
                }
            }

            HumToonGUIUtils.DoPopup<RenderFace>(materialEditor, PropContainer.CullMode, SurfaceOptionsStyles.RenderFace); // NOTE: RenderFaceで表裏の差を吸収

            HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.AlphaClip, SurfaceOptionsStyles.AlphaClip);

            if (PropContainer.AlphaClip.floatValue.IsOne())
                materialEditor.ShaderProperty(PropContainer.Cutoff, SurfaceOptionsStyles.Cutoff, 1);

            HumToonGUIUtils.DrawFloatToggleProperty(PropContainer.ReceiveShadows, SurfaceOptionsStyles.ReceiveShadow);
        }
    }
}
