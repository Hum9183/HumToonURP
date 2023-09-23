using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.NormalPropertiesContainer;

namespace HumToon.Editor
{
    public static class NormalStyles
    {
        public static readonly GUIContent NormalFoldout = EditorGUIUtility.TrTextContent(
            text: "Normal",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"These settings describe the look and feel of the surface itself.");

        public static readonly GUIContent NormalMap = EditorGUIUtility.TrTextContent(
            text: "Normal Map (RGB)",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Designates a Normal Map to create the illusion of bumps and dents on this Material's surface.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.BumpMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.BumpScale).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShaderKeywordStrings._NORMALMAP}");

        public static readonly GUIContent BumpScaleNotSupported = EditorGUIUtility.TrTextContent(
            text: "Bump scale is not supported on mobile platforms");

        public static readonly GUIContent FixNormalNow = EditorGUIUtility.TrTextContent(
            text: "Fix now",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Converts the assigned texture to be a normal map format.");
    }
}
