using System;
using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.ShadePropertyContainer;

namespace HumToon.Editor
{
    public static class ShadeStyles
    {
        public static readonly GUIContent ShadeFoldout = EditorGUIUtility.TrTextContent(
            text: "Shade",
            tooltip: String.Empty);

        public static readonly GUIContent UseFirstShade = EditorGUIUtility.TrTextContent(
            text: "Use First Shade",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.UseFirstShade).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_FIRST_SHADE}");

        public static readonly GUIContent FirstShadeMap = EditorGUIUtility.TrTextContent(
            text: "First Shade Map (RGB)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.FirstShadeMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.FirstShadeColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_FIRST_SHADE_MAP}");

        public static readonly GUIContent FirstShadeBorderPos = EditorGUIUtility.TrTextContent(
            text: "Border Pos",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.FirstShadeBorderPos).Prefix()}");

        public static readonly GUIContent FirstShadeBorderBlur = EditorGUIUtility.TrTextContent(
            text: "Border Blur",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.FirstShadeBorderBlur).Prefix()}");

        public static readonly GUIContent UseExFirstShade = EditorGUIUtility.TrTextContent(
            text: "Use Ex",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseExFirstShade).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_EX_FIRST_SHADE}");

        public static readonly GUIContent ExFirstShadeColor = EditorGUIUtility.TrTextContent(
            text: "Ex Color",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.ExFirstShadeColor).Prefix()}");

        public static readonly GUIContent ExFirstShadeWidth = EditorGUIUtility.TrTextContent(
            text: "Ex Width",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.ExFirstShadeWidth).Prefix()}");

        public static readonly GUIContent UseSecondShade = EditorGUIUtility.TrTextContent(
            text: "Use Second Shade",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.UseSecondShade).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_SECOND_SHADE}");

        public static readonly GUIContent SecondShadeMap = EditorGUIUtility.TrTextContent(
            text: "Second Shade Map (RGB)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.SecondShadeMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.SecondShadeColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._USE_SECOND_SHADE_MAP}");

        public static readonly GUIContent SecondShadeBorderPos = EditorGUIUtility.TrTextContent(
            text: "Border Pos",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.SecondShadeBorderPos).Prefix()}");

        public static readonly GUIContent SecondShadeBorderBlur = EditorGUIUtility.TrTextContent(
            text: "Border Blur",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.SecondShadeBorderBlur).Prefix()}");
    }
}
