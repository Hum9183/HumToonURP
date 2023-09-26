#pragma warning disable CS0414

using System;
using System.Reflection;
using Hum.HumToon.Editor.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using ID = Hum.HumToon.Editor.HeaderScope.Shade.ShadePropertiesID;

namespace Hum.HumToon.Editor.HeaderScope.Shade
{
    public class ShadeKeywords
    {
        // Shade Mode
        private bool _HUM_SHADE_MODE_POS_AND_BLUR;
        private bool _HUM_SHADE_MODE_RAMP;

        // Pos And Blur
        private bool _HUM_USE_FIRST_SHADE;
        private bool _HUM_USE_FIRST_SHADE_MAP;
        private bool _HUM_USE_EX_FIRST_SHADE;
        private bool _HUM_USE_SECOND_SHADE;
        private bool _HUM_USE_SECOND_SHADE_MAP;

        // Ramp
        private bool _HUM_USE_RAMP_SHADE;

        // Control Map
        private bool _HUM_USE_SHADE_CONTROL_MAP;

        public void Setup(Material material)
        {
            // WARNING: 呼び出し順に依存あり
            SetupShadeMode(material);
            SetupControlMap(material);
        }

        private void SetupShadeMode(Material material)
        {
            ShadeMode shadeMode = material.GetFloatEnum<ShadeMode>(ID.ShadeMode);
            switch (shadeMode)
            {
                case ShadeMode.PosAndBlur:
                    _HUM_SHADE_MODE_POS_AND_BLUR = true;
                    SetupPosAndBlur();
                    break;
                case ShadeMode.Ramp:
                    _HUM_SHADE_MODE_RAMP = true;
                    SetupRamp();
                    break;
                default:
                    throw new NotImplementedException(nameof(shadeMode));
            }

            return;

            void SetupPosAndBlur()
            {
                _HUM_USE_FIRST_SHADE = material.GetFloat(ID.UseFirstShade).ToBool();
                _HUM_USE_FIRST_SHADE_MAP = material.GetTexture(ID.FirstShadeMap) is not null;
                _HUM_USE_EX_FIRST_SHADE = material.GetFloat(ID.UseExFirstShade).ToBool() && _HUM_USE_FIRST_SHADE;
                _HUM_USE_SECOND_SHADE = material.GetFloat(ID.UseSecondShade).ToBool();
                _HUM_USE_SECOND_SHADE_MAP = material.GetTexture(ID.SecondShadeMap) is not null;
            }

            void SetupRamp()
            {
                bool useRampShade = material.GetFloat(ID.UseRampShade).ToBool();
                bool existsRampShadeMap = material.GetTexture(ID.RampShadeMap) is not null;
                _HUM_USE_RAMP_SHADE = useRampShade && existsRampShadeMap;
            }
        }

        private void SetupControlMap(Material material)
        {
            bool useShadeControlMap = material.GetFloat(ID.UseShadeControlMap).ToBool();
            bool existsShadeControlMap = material.GetTexture(ID.ShadeControlMap) is not null;
            bool isAtLeastOneShadeValid = _HUM_USE_FIRST_SHADE || _HUM_USE_SECOND_SHADE || _HUM_USE_RAMP_SHADE;
            _HUM_USE_SHADE_CONTROL_MAP = useShadeControlMap && existsShadeControlMap && isAtLeastOneShadeValid;
        }

        public void SetToMaterial(Material material)
        {
            var fieldInfos = typeof(ShadeKeywords).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                string keywordName = fieldInfo.Name;
                bool state = (bool)fieldInfo.GetValue(this);
                CoreUtils.SetKeyword(material, keywordName, state);
            }
        }
    }

}
