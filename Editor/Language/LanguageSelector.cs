using Hum.HumToon.Editor.Utils;

namespace Hum.HumToon.Editor.Language
{
    public static class LanguageSelector
    {
        public static string Select(string[] texts)
        {
            string result = string.Empty;

            if (texts.TryGetValue((int)HumToonLanguage.DefaultLang, out string defaultLangText))
            {
                if (string.IsNullOrEmpty(defaultLangText) is false)
                    result = defaultLangText;
            }

            if (texts.TryGetValue((int)HumToonLanguage.CurrentLang, out string currentLangText))
            {
                if (string.IsNullOrEmpty(currentLangText) is false)
                    result = currentLangText;
            }

            return result;
        }
    }
}
