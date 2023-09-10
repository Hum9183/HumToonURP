using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        private void DrawLight(Material material)
        {
            _materialEditor.ShaderProperty(_lightMaterialPropertyContainer.MainLightColorWeight, LightStyles.MainLightColorWeight);
            _materialEditor.ShaderProperty(_lightMaterialPropertyContainer.AdditionalLightColorWeight, LightStyles.AdditionalLightColorWeight);
        }
    }
}
