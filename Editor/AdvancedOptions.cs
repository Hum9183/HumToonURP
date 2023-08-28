using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        private void DrawAdvancedOptions(Material material)
        {
            _materialEditor.ShaderProperty(_litMatPropContainer.SpecularHighlights, LitStyles.Highlights);
            _materialEditor.ShaderProperty(_litMatPropContainer.EnvironmentReflections, LitStyles.Reflections);
            _materialEditor.IntSliderShaderProperty(_matPropContainer.QueueOffset, -QueueOffsetRange, QueueOffsetRange, HumToonStyles.QueueSlider);
            _materialEditor.EnableInstancingField();
        }
    }
}
