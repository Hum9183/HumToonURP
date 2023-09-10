using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public class SurfaceInputsValidator : IHeaderScopeValidator
    {
        private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private static void SetKeywords(Material material)
        {
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, material.GetTexture(BumpMap));
        }
    }
}
