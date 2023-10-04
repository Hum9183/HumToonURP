using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    public static class LanguageDrawer
    {
        public static void Draw()
        {
            HumToonLanguage.CurrentLang = (Language)DrawInternal();
        }

        private static int DrawInternal()
        {
            // TODO: Undo
            int newValue = EditorGUILayout.Popup(LanguageStyles.Language, (int)HumToonLanguage.CurrentLang, LanguageDisplayedOptionsGetter.Get<Language>());
            return newValue;
        }
    }
}
