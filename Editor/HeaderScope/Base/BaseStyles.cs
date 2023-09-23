using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using C = HumToon.Editor.Const;

namespace HumToon.Editor
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class BaseStyles
    {
        public static readonly GUIContent SurfaceInputsFoldout = EditorGUIUtility.TrTextContent(
            text: "Base",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"These settings describe the look and feel of the surface itself.");

        public static readonly GUIContent BaseMap = EditorGUIUtility.TrTextContent(
            text: "Base Map (RGBA)",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Specifies the base Material and/or Color of the surface. " +
                     $"If you’ve selected Transparent or Alpha Clipping under Surface Options, " +
                     $"your Material uses the Texture’s alpha channel or color.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(BasePropertiesContainer.BaseMap).Prefix()}{C.Ln}" +
                     $"{nameof(BasePropertiesContainer.BaseColor).Prefix()}");
    }
}
