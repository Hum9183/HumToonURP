using System;
using UnityEngine;

namespace Hum.HumToon.Editor.Utils
{
    public static class HumToonExtensionMethods
    {
        public static bool ToBool(this float value)
        {
            return value >= 0.5;
        }

        public static float ToFloat(this bool value)
        {
            return value ? 1.0f : 0.0f;
        }

        public static bool IsOne(this float value, float tolerance = 0.000000001f)
        {
            return Math.Abs(value - 1.0f) < tolerance;
        }

        public static string Prefix(this string value, string prefix = "_")
        {
            return $"{prefix}{value}";
        }

        public static T GetFloatEnum<T>(this Material material, int nameID)
            where T : Enum
        {
            var floatValue = material.GetFloat(nameID);
            var intValue = Convert.ToInt32(floatValue);
            return intValue.ToEnum<T>();
        }

        public static T ToEnum<T>(this int value)
            where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
