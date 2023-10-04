using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;
using C = Hum.HumToon.Editor.Utils.Const;
using L = Hum.HumToon.Editor.Language.LanguageSelector;
using P = Hum.HumToon.Editor.HeaderScopes.Emission.EmissionPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.Emission
{
    public static class EmissionStyles
    {
        public static GUIContent EmissionFoldout =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Emission", "発光", "自发光" })}",
                tooltip: string.Empty);

        public static readonly GUIContent UseEmission = EditorGUIUtility.TrTextContent(
            text: "Use Emission",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseEmission).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{EmissionKeywordNames._HUM_USE_EMISSION}");

        public static readonly GUIContent EmissionMap = EditorGUIUtility.TrTextContent(
            text: "Emission Map (RGB)",
            tooltip: $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.EmissionMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.EmissionColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{EmissionKeywordNames._HUM_USE_EMISSION_MAP}");

        public static readonly GUIContent EmissionIntensity = EditorGUIUtility.TrTextContent(
            text: "Intensity",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.EmissionIntensity).Prefix()}");

        public static readonly GUIContent EmissionFactorR = EditorGUIUtility.TrTextContent(
            text: "Factor R",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.EmissionFactorR).Prefix()}");

        public static readonly GUIContent EmissionFactorG = EditorGUIUtility.TrTextContent(
            text: "Factor G",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.EmissionFactorG).Prefix()}");

        public static readonly GUIContent EmissionFactorB = EditorGUIUtility.TrTextContent(
            text: "Factor B",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.EmissionFactorB).Prefix()}");

        public static readonly GUIContent OverrideEmissionColor = EditorGUIUtility.TrTextContent(
            text: "Override Emission Color",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.OverrideEmissionColor).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{EmissionKeywordNames._HUM_OVERRIDE_EMISSION_COLOR}");
    }
}
