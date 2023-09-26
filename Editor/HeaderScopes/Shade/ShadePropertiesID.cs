using Hum.HumToon.Editor.Utils;
using UnityEngine;
using P = Hum.HumToon.Editor.HeaderScopes.Shade.ShadePropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.Shade
{
    public static class ShadePropertiesID
    {
        public static readonly int ShadeMode          = ToID(nameof(P.ShadeMode));
        public static readonly int UseFirstShade      = ToID(nameof(P.UseFirstShade));
        public static readonly int FirstShadeMap      = ToID(nameof(P.FirstShadeMap));
        public static readonly int UseExFirstShade    = ToID(nameof(P.UseExFirstShade));
        public static readonly int UseSecondShade     = ToID(nameof(P.UseSecondShade));
        public static readonly int SecondShadeMap     = ToID(nameof(P.SecondShadeMap));
        public static readonly int UseShadeControlMap = ToID(nameof(P.UseShadeControlMap));
        public static readonly int UseRampShade       = ToID(nameof(P.UseRampShade));
        public static readonly int RampShadeMap       = ToID(nameof(P.RampShadeMap));
        public static readonly int ShadeControlMap    = ToID(nameof(P.ShadeControlMap));

        private static int ToID(string propNameWithoutPrefix)
        {
            return Shader.PropertyToID(propNameWithoutPrefix.Prefix());
        }
    }

}
