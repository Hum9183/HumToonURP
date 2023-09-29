namespace Hum.HumToon.Editor.Language
{
    public enum Language
    {
        English,
        Japanese,
        Chinese
    }

    public static class LanguageExtensionMethods
    {
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
    }
}
