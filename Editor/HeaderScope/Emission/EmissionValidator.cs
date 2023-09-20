using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.EmissionPropertyContainer;

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
            CoreUtils.SetKeyword(material, EmissionKeywords._HUM_USE_EMISSION, useFirstEmission);
        }
    }
}
