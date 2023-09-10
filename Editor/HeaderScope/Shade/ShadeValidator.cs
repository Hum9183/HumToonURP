using UnityEngine;
using UnityEngine.Rendering;

namespace HumToon.Editor
{
    public class ShadeValidator : IHeaderScopeValidator
    {
        private static readonly int FirstShadeMap = Shader.PropertyToID("_FirstShadeMap");
        private static readonly string _USE_FIRST_SHADE_MAP = "_USE_FIRST_SHADE_MAP";

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private static void SetKeywords(Material material)
        {
            bool existsFirstShadeMap = material.GetTexture(FirstShadeMap) is not null;
            CoreUtils.SetKeyword(material, _USE_FIRST_SHADE_MAP, existsFirstShadeMap);
        }
    }
}
