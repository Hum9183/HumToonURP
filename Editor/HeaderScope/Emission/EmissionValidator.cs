using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.EmissionPropertiesContainer;

namespace HumToon.Editor
{
    public class EmissionValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseEmission = Shader.PropertyToID($"{nameof(P.UseEmission).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useFirstEmission = material.GetFloat(IDUseEmission).ToBool();
            CoreUtils.SetKeyword(material, EmissionKeywordNames._HUM_USE_EMISSION, useFirstEmission);
        }
    }
}
