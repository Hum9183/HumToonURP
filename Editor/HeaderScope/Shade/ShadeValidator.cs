using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.ShadePropertyContainer;

namespace HumToon.Editor
{
    public class ShadeValidator : IHeaderScopeValidator
    {
        private static readonly int IDUseFirstShade = Shader.PropertyToID($"{nameof(P.UseFirstShade).Prefix()}");
        private static readonly int IDFirstShadeMap = Shader.PropertyToID($"{nameof(P.FirstShadeMap).Prefix()}");
        private static readonly int IDUseExFirstShade = Shader.PropertyToID($"{nameof(P.UseExFirstShade).Prefix()}");
        private static readonly int IDUseSecondShade = Shader.PropertyToID($"{nameof(P.UseSecondShade).Prefix()}");
        private static readonly int IDSecondShadeMap = Shader.PropertyToID($"{nameof(P.SecondShadeMap).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            bool useFirstShade = material.GetFloat(IDUseFirstShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_FIRST_SHADE, useFirstShade);

            bool existsFirstShadeMap = material.GetTexture(IDFirstShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_FIRST_SHADE_MAP, existsFirstShadeMap);

            bool useExFirstShade = material.GetFloat(IDUseExFirstShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_EX_FIRST_SHADE, useExFirstShade);

            bool useSecondShade = material.GetFloat(IDUseSecondShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_SECOND_SHADE, useSecondShade);

            bool existsSecondShadeMap = material.GetTexture(IDSecondShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._USE_SECOND_SHADE_MAP, existsSecondShadeMap);
        }
    }
}
