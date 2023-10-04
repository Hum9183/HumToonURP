using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    public static class LanguageStyles
    {
        public static GUIContent Language =>
            EditorGUIUtility.TrTextContent(
                text: $"{HumToonLanguage.Select(new string[] { "Language", "言語", "语言" })}");
    }
}
