using System;
using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.EmissionPropertyContainer;

namespace HumToon.Editor
{
    public static class EmissionStyles
    {
        public static readonly GUIContent EmissionFoldout = EditorGUIUtility.TrTextContent(
            text: "Emission",
            tooltip: String.Empty);

        public static readonly GUIContent UseEmission = EditorGUIUtility.TrTextContent(
            text: "Use First Emission",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseEmission).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{EmissionKeywords._HUM_USE_EMISSION}");
    }
}