using System;
using UnityEngine;
using UnityEngine.Rendering;
using P = HumToon.Editor.ShadePropertyContainer;

namespace HumToon.Editor
{
    public class ShadeValidator : IHeaderScopeValidator
    {
        private static readonly int IDShadeMode = Shader.PropertyToID($"{nameof(P.ShadeMode).Prefix()}");
        private static readonly int IDUseFirstShade = Shader.PropertyToID($"{nameof(P.UseFirstShade).Prefix()}");
        private static readonly int IDFirstShadeMap = Shader.PropertyToID($"{nameof(P.FirstShadeMap).Prefix()}");
        private static readonly int IDUseExFirstShade = Shader.PropertyToID($"{nameof(P.UseExFirstShade).Prefix()}");
        private static readonly int IDUseSecondShade = Shader.PropertyToID($"{nameof(P.UseSecondShade).Prefix()}");
        private static readonly int IDSecondShadeMap = Shader.PropertyToID($"{nameof(P.SecondShadeMap).Prefix()}");
        private static readonly int IDUseShadeControlMap = Shader.PropertyToID($"{nameof(P.UseShadeControlMap).Prefix()}");
        private static readonly int IDUseRampShade = Shader.PropertyToID($"{nameof(P.UseRampShade).Prefix()}");
        private static readonly int IDRampShadeMap = Shader.PropertyToID($"{nameof(P.RampShadeMap).Prefix()}");
        private static readonly int IDShadeControlMap = Shader.PropertyToID($"{nameof(P.ShadeControlMap).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private void SetKeywords(Material material)
        {
            // TODO:
            // Modeを切り替えても、Texがアサインされていると_HUM_USE_FIRST_SHADE_MAPなどが定義されてしまう。
            // やり方を考える。

            ShadeMode shadeMode = material.GetFloatEnum<ShadeMode>(IDShadeMode);
            (bool posAndBlur, bool ramp) = (false, false);
            switch (shadeMode)
            {
                case ShadeMode.PosAndBlur:
                    posAndBlur = true;
                    break;
                case ShadeMode.Ramp:
                    ramp = true;
                    break;
                default:
                    throw new NotImplementedException(nameof(shadeMode));
            }
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_SHADE_MODE_POS_AND_BLUR, posAndBlur);
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_SHADE_MODE_RAMP, ramp);

            // Pos And Blur

            bool useFirstShade = material.GetFloat(IDUseFirstShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_FIRST_SHADE, useFirstShade);

            bool existsFirstShadeMap = material.GetTexture(IDFirstShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_FIRST_SHADE_MAP, existsFirstShadeMap);

            bool useExFirstShade = material.GetFloat(IDUseExFirstShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_EX_FIRST_SHADE, useExFirstShade);

            bool useSecondShade = material.GetFloat(IDUseSecondShade).ToBool();
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_SECOND_SHADE, useSecondShade);

            bool existsSecondShadeMap = material.GetTexture(IDSecondShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_SECOND_SHADE_MAP, existsSecondShadeMap);

            // Ramp

            bool useRampShade = material.GetFloat(IDUseRampShade).ToBool();
            bool existsRampShadeMap = material.GetTexture(IDRampShadeMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_RAMP_SHADE, useRampShade && existsRampShadeMap);

            // Control Map

            bool useShadeControlMap = material.GetFloat(IDUseShadeControlMap).ToBool();
            bool existsShadeControlMap = material.GetTexture(IDShadeControlMap) is not null;
            CoreUtils.SetKeyword(material, ShadeKeywords._HUM_USE_SHADE_CONTROL_MAP, useShadeControlMap && existsShadeControlMap);
        }
    }
}
