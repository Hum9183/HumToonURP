using System;
using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.ShadePropertiesContainer;

namespace HumToon.Editor
{
    public static class ShadeStyles
    {
        private const string DescriptionIfIsNot = "If texture is not assigned, it refers to Base Map.";

        public static readonly GUIContent ShadeFoldout = EditorGUIUtility.TrTextContent(
            text: "Shade",
            tooltip: String.Empty);

        // Shade Mode

        public static readonly GUIContent ShadeMode = EditorGUIUtility.TrTextContent(
            text: "Shade Mode",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.ShadeMode).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keywords}{C.Ln}" +
                     $"{ShadeKeywords._HUM_SHADE_MODE_POS_AND_BLUR}{C.Ln}" +
                     $"{ShadeKeywords._HUM_SHADE_MODE_RAMP}");

        // Pos And Blur

        public static readonly GUIContent UseFirstShade = EditorGUIUtility.TrTextContent(
            text: "Use First Shade",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.UseFirstShade).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._HUM_USE_FIRST_SHADE}");

        public static readonly GUIContent FirstShadeMap = EditorGUIUtility.TrTextContent(
            text: "First Shade Map (RGB)",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"{DescriptionIfIsNot}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.FirstShadeMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.FirstShadeColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._HUM_USE_FIRST_SHADE_MAP}");

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
                     $"{ShadeKeywords._HUM_USE_EX_FIRST_SHADE}");

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
                     $"{ShadeKeywords._HUM_USE_SECOND_SHADE}");

        public static readonly GUIContent SecondShadeMap = EditorGUIUtility.TrTextContent(
            text: "Second Shade Map (RGB)",
            tooltip:  $"{C.Description}{C.Ln}" +
                      $"{DescriptionIfIsNot}{C.Ln}" +
                      $"{C.Ln}" +
                      $"{C.Properties}{C.Ln}" +
                      $"{nameof(P.SecondShadeMap).Prefix()}{C.Ln}" +
                      $"{nameof(P.SecondShadeColor).Prefix()}{C.Ln}" +
                      $"{C.Ln}" +
                      $"{C.Keyword}{C.Ln}" +
                      $"{ShadeKeywords._HUM_USE_SECOND_SHADE_MAP}");

        public static readonly GUIContent SecondShadeBorderPos = EditorGUIUtility.TrTextContent(
            text: "Border Pos",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.SecondShadeBorderPos).Prefix()}");

        public static readonly GUIContent SecondShadeBorderBlur = EditorGUIUtility.TrTextContent(
            text: "Border Blur",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.SecondShadeBorderBlur).Prefix()}");

        // Ramp

        public static readonly GUIContent UseRampShade = EditorGUIUtility.TrTextContent(
            text: "Use Ramp Shade",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseRampShade).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._HUM_USE_RAMP_SHADE}");

        public static readonly GUIContent RampShadeMap = EditorGUIUtility.TrTextContent(
            text: "Ramp Shade Map (RGB)",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.RampShadeMap).Prefix()}");

        // Control Map

        public static readonly GUIContent UseShadeControlMap = EditorGUIUtility.TrTextContent(
            text: "Use Shade Control Map",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseShadeControlMap).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShadeKeywords._HUM_USE_SHADE_CONTROL_MAP}");

        public static readonly GUIContent ShadeControlMap = EditorGUIUtility.TrTextContent(
            text: "Shade Control Map (R)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.ShadeControlMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.ShadeControlMapIntensity).Prefix()}");
    }
}
