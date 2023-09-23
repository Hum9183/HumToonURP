using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.RimLightPropertiesContainer;

namespace HumToon.Editor
{
    public class RimLightValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseRimLight = Shader.PropertyToID(nameof(P.UseRimLight).Prefix());
        private static readonly int IDRimLightMap = Shader.PropertyToID(nameof(P.RimLightMap).Prefix());

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            // TODO:
            // Useをオフにしても、Texがアサインされていると_HUM_USE_RIM_LIGHT_MAPが定義されてしまう。
            // やり方を考える。

            bool useRimLight = material.GetFloat(IDUseRimLight).ToBool();
            CoreUtils.SetKeyword(material, RimLightKeywordNames._HUM_USE_RIM_LIGHT, useRimLight);

            bool existsRimLightMap = material.GetTexture(IDRimLightMap) is not null;
            CoreUtils.SetKeyword(material, RimLightKeywordNames._HUM_USE_RIM_LIGHT_MAP, existsRimLightMap);
        }
    }
}
