using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    public static class LanguageDrawer
    {
        public static void Draw()
        {
            HumToonLanguage.CurrentLang = (Language)DrawInternal(HumToonLanguage.CurrentLang);
        }

        private static int DrawInternal(Language currentLang)
        {
            int newValue = EditorGUILayout.Popup(LanguageStyles.Language, (int)currentLang, LanguageDisplayedOptionsGetter.Get<Language>(currentLang));
            return newValue;
        }
    }
}
