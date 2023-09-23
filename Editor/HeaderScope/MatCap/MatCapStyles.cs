using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.MatCapPropertiesContainer;

namespace HumToon.Editor
{
    public static class MatCapStyles
    {
        public static readonly GUIContent MatCapFoldout = EditorGUIUtility.TrTextContent(
            text: "Mat Cap",
            tooltip: string.Empty);

        public static readonly GUIContent UseMatCap = EditorGUIUtility.TrTextContent(
            text: "Use Mat Cap",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseMatCap).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{MatCapKeywords._HUM_USE_MAT_CAP}");

        public static readonly GUIContent MatCapMap = EditorGUIUtility.TrTextContent(
            text: "Mat Cap Map (RGB)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.MatCapColor).Prefix()}");

        public static readonly GUIContent MatCapIntensity = EditorGUIUtility.TrTextContent(
            text: "Intensity",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapIntensity).Prefix()}");

        public static readonly GUIContent MatCapCorrectPerspectiveDistortion = EditorGUIUtility.TrTextContent(
            text: "Correct Persp Distortion",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapCorrectPerspectiveDistortion).Prefix()}");

        public static readonly GUIContent MatCapStabilizeCameraZRotation = EditorGUIUtility.TrTextContent(
            text: "Stabilize Camera Z Rotation",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapStabilizeCameraZRotation).Prefix()}");

        public static readonly GUIContent MatCapMainLightEffectiveness = EditorGUIUtility.TrTextContent(
            text: "Main Light Effectiveness",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapMainLightEffectiveness).Prefix()}");
    }
}
