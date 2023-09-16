using System;
using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;

namespace HumToon.Editor
{
    public static class ShadeStyles
    {
        private static readonly ShadePropertyContainer P = new ShadePropertyContainer(null);

        public static readonly GUIContent ShadeFoldout = EditorGUIUtility.TrTextContent(
            text: "Shade",
            tooltip: String.Empty);

        public static readonly GUIContent FirstShadeMap = EditorGUIUtility.TrTextContent(
            text: "First Shade Map",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.FirstShadeMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.FirstShadeColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_FIRST_SHADE_MAP}");

        public static readonly GUIContent FirstShadeBorderPos = EditorGUIUtility.TrTextContent(
            text: "First Shade Border Pos",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.FirstShadeBorderPos).Prefix()}");

        public static readonly GUIContent FirstShadeBorderBlur = EditorGUIUtility.TrTextContent(
            text: "First Shade Border Blur",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.FirstShadeBorderBlur).Prefix()}");
    }
}
