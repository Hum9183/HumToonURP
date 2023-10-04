using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    public static class HumToonLanguage
    {
        private const Language DefaultLang = Language.English;
        private const string EditorUserSettingsConfigName = "HumToonLanguage";

        private static Language currentLanguage;

        private static GUIContent LanguageLabel =>
            EditorGUIUtility.TrTextContent(
                text: $"{Select(new string[] { "Language", "言語", "语言" })}");

        public static string Select(string[] texts)
        {
            string result = string.Empty;

            if (texts.TryGetValue((int)DefaultLang, out string defaultLangText))
            {
                if (string.IsNullOrEmpty(defaultLangText) is false)
                    result = defaultLangText;
            }

            if (texts.TryGetValue((int)currentLanguage, out string currentLangText))
            {
                if (string.IsNullOrEmpty(currentLangText) is false)
                    result = currentLangText;
            }

            return result;
        }

        /// <summary>
        /// Draw language GUI
        /// </summary>
        public static void Draw()
        {
            int currentLang = GetFromEditorUserSettings();
            int newLang = DrawInternal(currentLang);
            SetEditorUserSettings(newLang);
            SetCurrentLanguage(newLang);
        }

        private static int GetFromEditorUserSettings()
        {
            string langStr = EditorUserSettings.GetConfigValue(EditorUserSettingsConfigName); // e.g. "0", "1", "2"
            langStr ??= ((int)DefaultLang).ToString();

            bool success = int.TryParse(langStr, out int langInt);
            return success ? langInt : (int)DefaultLang;
        }

        private static int DrawInternal(int lang)
        {
            // TODO: Undo
            int newValue = EditorGUILayout.Popup(LanguageLabel, lang, ((Language)lang).DisplayedOptions());
            return newValue;
        }

        private static void SetEditorUserSettings(int lang)
        {
            EditorUserSettings.SetConfigValue(EditorUserSettingsConfigName, lang.ToString());
        }

        private static void SetCurrentLanguage(int newLang)
        {
            currentLanguage = (Language)newLang;
        }
    }
}
