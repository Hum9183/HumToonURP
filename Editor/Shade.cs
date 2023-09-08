using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        private void DrawShade(Material material)
        {
            _materialEditor.TexturePropertySingleLine(ShadeStyles.FirstShadeMap, _shadeMaterialPropertyContainer.FirstShadeMap, _shadeMaterialPropertyContainer.FirstShadeColor);
            _materialEditor.ShaderProperty(_shadeMaterialPropertyContainer.FirstShadeBorderPos, ShadeStyles.FirstShadeBorderPos);
            _materialEditor.ShaderProperty(_shadeMaterialPropertyContainer.FirstShadeBorderBlur, ShadeStyles.FirstShadeBorderBlur);
        }
    }
}
