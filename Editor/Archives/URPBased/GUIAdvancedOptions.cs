using UnityEditor.Rendering;
using UnityEngine;
// using static Unity.Rendering.Universal.ShaderUtils;

namespace Hum.HumToon.Editor.Archives.URPBased
{
    internal partial class HumToonGUI
    {
        private void DrawAdvancedOptions(Material material)
        {
            _materialEditor.IntSliderShaderProperty(_matProps.QueueOffset, -QueueOffsetRange, QueueOffsetRange, Styles.queueSlider);
            _materialEditor.EnableInstancingField();
        }
    }
}
