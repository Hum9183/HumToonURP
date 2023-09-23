using System;
using UnityEngine;

namespace HumToon.Editor
{
    public static class Utils
    {
        public static bool GetPreserveSpecular(Material material, TransparentBlendMode transparentBlendMode)
        {
            // Lift alpha multiply from ROP to shader by setting pre-multiplied _SrcBlend mode.
            // The intent is to do different blending for diffuse and specular in shader.
            // ref: http://advances.realtimerendering.com/other/2016/naughty_dog/NaughtyDog_TechArt_Final.pdf
            return material.GetFloat(HumToonPropertyNames.BlendModePreserveSpecular).ToBool()
                   && transparentBlendMode != TransparentBlendMode.Multiply
                   && transparentBlendMode != TransparentBlendMode.Premultiply;
        }

        public static bool IsNotNull(object obj)
        {
            return obj is not null;
        }

        public static string InsertSpaceBeforeUppercase(string source)
        {
            string result = string.Empty;
            foreach (char VARIABLE in source)
            {
                if (char.IsUpper(VARIABLE))
                    result += " ";
                result += VARIABLE;
            }

            // NOTE: sourceの先頭文字が大文字だった場合、先頭にスペースが入ってしまうため、削除する。
            result = result.TrimStart();
            return result;
        }
    }
}
