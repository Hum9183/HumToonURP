using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;
using C = Hum.HumToon.Editor.Utils.Const;
using L = Hum.HumToon.Editor.Language.HumToonLanguage;
using P = Hum.HumToon.Editor.HeaderScopes.MatCap.MatCapPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public static class MatCapStyles
    {
        public static GUIContent MatCapFoldout =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Mat Cap", "マットキャップ", "" })}",
                tooltip: string.Empty);

        public static readonly GUIContent UseMatCap = EditorGUIUtility.TrTextContent(
            text: "Use Mat Cap",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseMatCap).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{MatCapKeywordNames._HUM_USE_MAT_CAP}");

        public static readonly GUIContent MatCapMap = EditorGUIUtility.TrTextContent(
            text: "Mat Cap Map (RGB)",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Mat Cap Map must always be assigned. " +
                     $"If not assigned, " +
                     $"the keyword will be invalid and Mat Cap will be invalid.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.MatCapColor).Prefix()}");

        public static readonly GUIContent MatCapIntensity = EditorGUIUtility.TrTextContent(
            text: "Intensity",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapIntensity).Prefix()}");

        public static readonly GUIContent MatCapMapMipLevel = EditorGUIUtility.TrTextContent(
            text: "Mip Level",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapMapMipLevel).Prefix()}");

        public static readonly GUIContent MatCapCorrectPerspectiveDistortion = EditorGUIUtility.TrTextContent(
            text: "Correct Persp Distortion",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapCorrectPerspectiveDistortion).Prefix()}");

        public static readonly GUIContent MatCapStabilizeCameraZRotation = EditorGUIUtility.TrTextContent(
            text: "Stabilize Camera Z Rotation",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapStabilizeCameraZRotation).Prefix()}");

        public static readonly GUIContent MatCapMask = EditorGUIUtility.TrTextContent(
            text: "Mask",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.MatCapMask).Prefix()}{C.Ln}" +
                     $"{nameof(P.MatCapMaskIntensity).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{MatCapKeywordNames._HUM_USE_MAT_CAP_MASK}");

        public static readonly GUIContent MatCapMainLightEffectiveness = EditorGUIUtility.TrTextContent(
            text: "Main Light Effectiveness",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MatCapMainLightEffectiveness).Prefix()}");
    }
}
