using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.RimLightPropertyContainer;

namespace HumToon.Editor
{
    public static class RimLightStyles
    {
        public static readonly GUIContent RimLightFoldout = EditorGUIUtility.TrTextContent(
            text: "Rim Light",
            tooltip: string.Empty);

        public static readonly GUIContent UseRimLight = EditorGUIUtility.TrTextContent(
            text: "Use Rim Light",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseRimLight).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{RimLightKeywords._HUM_USE_RIM_LIGHT}");
    }
}
