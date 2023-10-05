using Hum.HumToon.Editor.Utils;

namespace Hum.HumToon.Editor.Language
{
    public static class LanguageSelector
    {
        public static string Select(string[] texts)
        {
            return SelectInternal(texts, HumToonLanguage.DefaultLang, HumToonLanguage.CurrentLang);
        }

        private static string SelectInternal(string[] texts, Language defaultLang, Language currentLang)
        {
            string result = string.Empty;

            if (texts.TryGetValue((int)defaultLang, out string defaultLangText))
            {
                if (string.IsNullOrEmpty(defaultLangText) is false)
                    result = defaultLangText;
            }

            if (texts.TryGetValue((int)currentLang, out string currentLangText))
            {
                if (string.IsNullOrEmpty(currentLangText) is false)
                    result = currentLangText;
            }

            return result;
        }
    }
}
