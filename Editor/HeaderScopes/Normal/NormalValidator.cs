using Hum.HumToon.Editor.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using P = Hum.HumToon.Editor.HeaderScopes.Normal.NormalPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.Normal
{
    public class NormalValidator : IHeaderScopeValidator
    {
        private static readonly int IDBumpMap = Shader.PropertyToID($"{nameof(P.BumpMap).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private static void SetKeywords(Material material)
        {
            bool normalMapExists = material.GetTexture(IDBumpMap) is not null;
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, normalMapExists);
        }
    }
}
