using System;
using UnityEditor;
using UnityEngine;
using C = HumToon.Editor.Const;

namespace HumToon.Editor
{
    public static class LightStyles
    {
        private static readonly LightPropertyContainer P = new LightPropertyContainer(null);

        public static readonly GUIContent LightFoldout = EditorGUIUtility.TrTextContent(
            text: "Light",
            tooltip: String.Empty);

        public static readonly GUIContent MainLightColorWeight = EditorGUIUtility.TrTextContent(
            text: "Color Weight",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.MainLightColorWeight).Prefix()}");

        public static readonly GUIContent MainLightUpperLimit = EditorGUIUtility.TrTextContent(
            text: "Upper Limit",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseMainLightUpperLimit).Prefix()}{C.Ln}" +
                     $"{nameof(P.MainLightUpperLimit).Prefix()}");

        public static readonly GUIContent MainLightLowerLimit = EditorGUIUtility.TrTextContent(
            text: "Lower Limit",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.UseMainLightLowerLimit).Prefix()}{C.Ln}" +
                     $"{nameof(P.MainLightLowerLimit).Prefix()}");

        public static readonly GUIContent AdditionalLightsColorWeight = EditorGUIUtility.TrTextContent(
            text: "Color Weight",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.AdditionalLightsColorWeight).Prefix()}");
    }
}
