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

        public static readonly GUIContent RimLightMap = EditorGUIUtility.TrTextContent(
            text: "Rim Light Map (RGB)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.RimLightMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.RimLightColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{RimLightKeywords._HUM_USE_RIM_LIGHT_MAP}");

        public static readonly GUIContent RimLightBorderPos = EditorGUIUtility.TrTextContent(
            text: "Border Pos",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.RimLightBorderPos).Prefix()}");

        public static readonly GUIContent RimLightBorderBlur = EditorGUIUtility.TrTextContent(
            text: "Border Blur",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.RimLightBorderBlur).Prefix()}");

        public static readonly GUIContent RimLightMainLightEffectiveness = EditorGUIUtility.TrTextContent(
            text: "Main Light Effectiveness",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.RimLightMainLightEffectiveness).Prefix()}");
    }
}
