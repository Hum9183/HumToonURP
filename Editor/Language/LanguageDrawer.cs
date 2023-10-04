using UnityEditor;

namespace Hum.HumToon.Editor.Language
{
    public class LanguageDrawer
    {
        private const string EditorUserSettingsConfigName = "HumToonLanguage";

        public Language Draw(Language defaultLang)
        {
            int currentLang = GetFromEditorUserSettings(defaultLang);
            int newLang = DrawInternal(currentLang);
            SetEditorUserSettings(newLang);
            return (Language)newLang;;
        }

        private int GetFromEditorUserSettings(Language defaultLang)
        {
            string langStr = EditorUserSettings.GetConfigValue(EditorUserSettingsConfigName); // e.g. "0", "1", "2"
            langStr ??= ((int)defaultLang).ToString();

            bool success = int.TryParse(langStr, out int langInt);
            return success ? langInt : (int)defaultLang;
        }

        private int DrawInternal(int currentLang)
        {
            // TODO: Undo
            int newValue = EditorGUILayout.Popup(LanguageStyles.Language, currentLang, HumToonLanguage.DisplayedOptions<Language>());
            return newValue;
        }

        private void SetEditorUserSettings(int newLang)
        {
            EditorUserSettings.SetConfigValue(EditorUserSettingsConfigName, newLang.ToString());
        }
    }
}
