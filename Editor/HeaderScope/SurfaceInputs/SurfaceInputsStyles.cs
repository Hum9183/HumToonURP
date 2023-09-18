using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using C = HumToon.Editor.Const;
using P = HumToon.Editor.SurfaceInputsPropertyContainer;

namespace HumToon.Editor
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class SurfaceInputsStyles
    {
        public static readonly GUIContent SurfaceInputsFoldout = EditorGUIUtility.TrTextContent(
            text: "Surface Inputs",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"These settings describe the look and feel of the surface itself.");

        public static readonly GUIContent BaseMap = EditorGUIUtility.TrTextContent(
            text: "Base Map",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Specifies the base Material and/or Color of the surface. " +
                     $"If you’ve selected Transparent or Alpha Clipping under Surface Options, " +
                     $"your Material uses the Texture’s alpha channel or color.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.BaseMap).Prefix()}{C.Ln}" +
                     $"{nameof(P.BaseColor).Prefix()}");

        public static readonly GUIContent NormalMap = EditorGUIUtility.TrTextContent(
            text: "Normal Map",
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
