using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;
using C = Hum.HumToon.Editor.Utils.Const;
using L = Hum.HumToon.Editor.Language.LanguageSelector;
using P = Hum.HumToon.Editor.HeaderScopes.Light.LightPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.Light
{
    public static class LightStyles
    {
        public static GUIContent LightFoldout =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Light", "ライト", "光源" })}",
                tooltip: String.Empty);

        public static readonly GUIContent MainLightColorWeight = EditorGUIUtility.TrTextContent(
            text: "Weight",
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
            text: "Weight",
            tooltip: $"{C.Property}{C.Ln}" +
                     $"{nameof(P.AdditionalLightsColorWeight).Prefix()}");
    }
}
