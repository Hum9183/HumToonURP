using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;

namespace HumToon.Editor
{
    public static class MatCapStyles
    {
        private static readonly MatCapPropertyContainer P = new MatCapPropertyContainer(null);

        public static readonly GUIContent MatCapFoldout = EditorGUIUtility.TrTextContent(
            text: "Mat Cap",
            tooltip: string.Empty);

        public static readonly GUIContent UseMatCap = EditorGUIUtility.TrTextContent(
            text: "Use Mat Cap",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseMatCap).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{MatCapKeywords._USE_MAT_CAP}");

        public static readonly GUIContent MatCapMap = EditorGUIUtility.TrTextContent(
            text: "Mat Cap Map",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.MatCapColor).Prefix()}");

        public static readonly GUIContent MatCapMainLightEffectiveness = EditorGUIUtility.TrTextContent(
            text: "Main Light Effectiveness",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapMainLightEffectiveness).Prefix()}");
    }
}
