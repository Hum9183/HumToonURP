using Hum.HumToon.Editor.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using P = Hum.HumToon.Editor.HeaderScopes.RimLight.RimLightPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.RimLight
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
            bool useRimLight = material.GetFloat(IDUseRimLight).ToBool();
            CoreUtils.SetKeyword(material, RimLightKeywordNames._HUM_USE_RIM_LIGHT, useRimLight);

            bool existsRimLightMap = material.GetTexture(IDRimLightMap) is not null;
            CoreUtils.SetKeyword(material, RimLightKeywordNames._HUM_USE_RIM_LIGHT_MAP, existsRimLightMap && useRimLight);
        }
    }
}
