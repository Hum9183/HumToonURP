using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

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
            string langStr = EditorUserSettings.GetConfigValue(EditorUserSettingsConfigName); // e.g. "0", "1", "2"
            langStr ??= ((int)DefaultLang).ToString();

            bool success = int.TryParse(langStr, out int langInt);
            int lang = success ? langInt : (int)DefaultLang;
            return (Language)lang;
        }

        private static void SetEditorUserSettings(Language newLang)
        {
            EditorUserSettings.SetConfigValue(EditorUserSettingsConfigName, ((int)newLang).ToString());
        }
    }

}
