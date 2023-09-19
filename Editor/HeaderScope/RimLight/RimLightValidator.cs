using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.RimLightPropertyContainer;

namespace HumToon.Editor
{
    public class RimLightValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseRimLight = Shader.PropertyToID(nameof(P.UseRimLight).Prefix());

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useRimLight = material.GetFloat(IDUseRimLight).ToBool();
            CoreUtils.SetKeyword(material, RimLightKeywords._HUM_USE_RIM_LIGHT, useRimLight);
        }
    }
}
