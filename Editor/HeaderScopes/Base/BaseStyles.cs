using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;
using C = Hum.HumToon.Editor.Utils.Const;
using L = Hum.HumToon.Editor.Language.LanguageSelector;

namespace Hum.HumToon.Editor.HeaderScopes.Base
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class BaseStyles
    {
        public static GUIContent SurfaceInputsFoldout =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Base", "ベース", "基础" })}",
                tooltip: $"{C.Description}{C.Ln}" +
                         $"These settings describe the look and feel of the surface itself.");

        public static GUIContent BaseMap =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "BaseMap", "ベースマップ", "基础贴图" })}",
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
