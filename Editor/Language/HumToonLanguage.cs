using System;

namespace Hum.HumToon.Editor.Language
{
    public static class HumToonLanguage
    {
        private const Language DefaultLang = Language.English;
        private static Language currentLanguage;

        private static readonly LanguageDrawer LanguageDrawer = new LanguageDrawer();
        private static readonly LanguageSelector LanguageSelector = new LanguageSelector();
        private static readonly LanguageDisplayedOptionsGetter LanguageDisplayedOptionsGetter = new LanguageDisplayedOptionsGetter();

        public static void Draw()
        {
            currentLanguage = LanguageDrawer.Draw(DefaultLang);
        }

        public static string Select(string[] texts)
        {
            return LanguageSelector.Select(texts, DefaultLang, currentLanguage);
        }

        public static string[] DisplayedOptions<T>()
            where T: Enum
        {
            return LanguageDisplayedOptionsGetter.GetDisplayedOptions<T>(currentLanguage);
        }
    }
}
