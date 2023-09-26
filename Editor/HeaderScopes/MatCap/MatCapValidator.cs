using Hum.HumToon.Editor.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using P = Hum.HumToon.Editor.HeaderScopes.MatCap.MatCapPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public class MatCapValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseMatCap = Shader.PropertyToID(nameof(P.UseMatCap).Prefix());
        private static readonly int IDMatCapMap = Shader.PropertyToID(nameof(P.MatCapMap).Prefix());

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useMatCap = material.GetFloat(IDUseMatCap).ToBool();
            bool existsMatCapMap = material.GetTexture(IDMatCapMap) is not null;
            CoreUtils.SetKeyword(material, MatCapKeywordNames._HUM_USE_MAT_CAP, useMatCap && existsMatCapMap);
        }
    }
}
