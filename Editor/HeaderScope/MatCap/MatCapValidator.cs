using UnityEngine;
using UnityEngine.Rendering;

namespace HumToon.Editor
{
    public class MatCapValidator : IHeaderScopeValidator
    {
        private static readonly string _USE_MAT_CAP = "_USE_MAT_CAP";
        private static readonly int UseMatCap = Shader.PropertyToID("_UseMatCap");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useMatCap = material.GetFloat(UseMatCap).ToBool();
            CoreUtils.SetKeyword(material, _USE_MAT_CAP, useMatCap);
        }
    }
}
