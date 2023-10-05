using System;
using UnityEditor;

namespace Hum.HumToon.Editor.Language
{
    public static class HumToonLanguage
    {
        public static Language CurrentLang
        {
            get => GetFromEditorUserSettings();
            set => SetEditorUserSettings(value);
        }

        public static readonly Language DefaultLang = Language.English;
        private const string EditorUserSettingsConfigName = "HumToonLanguage";

        private static Language GetFromEditorUserSettings()
        {
            string langStr = EditorUserSettings.GetConfigValue(EditorUserSettingsConfigName);
            bool success = Enum.TryParse<Language>(langStr, out var lang);
            return success ? lang : DefaultLang;
        }

        private static void SetEditorUserSettings(Language newLang)
        {
            EditorUserSettings.SetConfigValue(EditorUserSettingsConfigName, newLang.ToString());
        }
    }

}
