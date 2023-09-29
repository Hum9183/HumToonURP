using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor
{
    public static class HumToonLanguage
    {
        private const Language DefaultLang = Language.English;
        private const string ConfigName = "HumToonLanguage";
        // private static readonly string[] DisplayedOptions = Enum.GetNames(typeof(Language));

        private static Language currentLanguage;

        private static GUIContent LanguageLabel =>
            EditorGUIUtility.TrTextContent(
                text: $"{Select(new string[] { "Language", "言語", "语言" })}");

        public enum Language
        {
            English,
            Japanese,
            Chinese
        }

        public static string[] DisplayedOptions(this Language value)
        {
            var displayedOptionsArray =  new string[][]
            {
                new string[] { "English", "Japanese", "Chinese" },
                new string[] { "英語", "日本語", "中国語" },
                new string[] { "英语", "日语", "中文" },
            };

            return displayedOptionsArray[(int)value];
        }

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
        /// <returns>Changed language</returns>
        public static void Draw()
        {
            int currentLang = GetFromEditorUserSettings();
            int newLang = DrawInternal(currentLang);
            SetToEditorUserSettings(newLang);
        }

        private static int GetFromEditorUserSettings()
        {
            string langStr = EditorUserSettings.GetConfigValue(ConfigName); // e.g. "0", "1", "2"
            langStr ??= ((int)DefaultLang).ToString();

            bool success = Int32.TryParse(langStr, out int langInt);
            return success ? langInt : (int)DefaultLang;
        }

        private static int DrawInternal(int lang)
        {
            // TODO: Undo
            int newValue = EditorGUILayout.Popup(LanguageLabel, lang, ((Language)lang).DisplayedOptions());
            currentLanguage = (Language)newValue;
            return newValue;
        }

        private static void SetToEditorUserSettings(int lang)
        {
            EditorUserSettings.SetConfigValue(ConfigName, lang.ToString());
            AssetDatabase.SaveAssets();
        }
    }
}
