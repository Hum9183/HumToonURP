using UnityEngine;
using P = HumToon.Editor.ShadePropertiesContainer;

namespace HumToon.Editor
{
    public class ShadeValidator : IHeaderScopeValidator
    {
        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            var shadeKeywords = new ShadeKeywords();
            shadeKeywords.Setup(material);
            shadeKeywords.SetToMaterial(material);
        }
    }
}
