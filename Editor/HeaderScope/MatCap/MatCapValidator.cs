using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.MatCapPropertyContainer;

namespace HumToon.Editor
{
    public class MatCapValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseMatCap = Shader.PropertyToID(nameof(P.UseMatCap).Prefix());

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useMatCap = material.GetFloat(IDUseMatCap).ToBool();
            CoreUtils.SetKeyword(material, MatCapKeywords._USE_MAT_CAP, useMatCap);
        }
    }
}
