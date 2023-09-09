using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        private static readonly int FirstShadeMap = Shader.PropertyToID("_FirstShadeMap");

        private void DrawShade(Material material)
        {
            _materialEditor.TexturePropertySingleLine(ShadeStyles.FirstShadeMap, _shadeMaterialPropertyContainer.FirstShadeMap, _shadeMaterialPropertyContainer.FirstShadeColor);
            _materialEditor.ShaderProperty(_shadeMaterialPropertyContainer.FirstShadeBorderPos, ShadeStyles.FirstShadeBorderPos);
            _materialEditor.ShaderProperty(_shadeMaterialPropertyContainer.FirstShadeBorderBlur, ShadeStyles.FirstShadeBorderBlur);
        }

        private void ValidateMaterial_Shade(Material material)
        {
            bool existsFirstShadeMap = material.GetTexture(FirstShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords.UseFirstShadeMap, existsFirstShadeMap);
        }
    }
}
