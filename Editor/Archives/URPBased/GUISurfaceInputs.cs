using UnityEngine;
// using static Unity.Rendering.Universal.ShaderUtils;

namespace Hum.HumToon.Editor.Archives.URPBased
{
    internal partial class HumToonGUI
    {
        private void DrawSurfaceInputs(Material material)
        {
            _materialEditor.TexturePropertySingleLine(Styles.baseMap, _matProps.BaseMap, _matProps.BaseColor);
            // _materialEditor.TextureScaleOffsetProperty(_matProps.BaseMap);
        }
    }
}
