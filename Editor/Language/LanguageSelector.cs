using Hum.HumToon.Editor.Utils;

namespace Hum.HumToon.Editor.Language
{
    public class LanguageSelector
    {
        public string Select(string[] texts, Language defaultLang, Language currentLanguage)
        {
            string result = string.Empty;

            if (texts.TryGetValue((int)defaultLang, out string defaultLangText))
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
    }
}
