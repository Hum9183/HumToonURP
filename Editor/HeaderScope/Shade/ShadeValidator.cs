using UnityEngine;
using UnityEngine.Rendering;

namespace HumToon.Editor
{
    public class ShadeValidator : IHeaderScopeValidator
    {
        private static readonly ShadePropertyContainer P = new ShadePropertyContainer(null);
        private static readonly int IDFirstShadeMap = Shader.PropertyToID($"{nameof(P.FirstShadeMap).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool existsFirstShadeMap = material.GetTexture(IDFirstShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_FIRST_SHADE_MAP, existsFirstShadeMap);
        }
    }
}
