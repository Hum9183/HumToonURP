using System;

namespace Hum.HumToon.Editor.Language
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class DisplayNameLanguageAttributeBase : Attribute
    {
        public string DisplayName { get; }
        public abstract Language Enum { get; }

        protected DisplayNameLanguageAttributeBase(string displayName)
        {
            DisplayName = displayName;
        }
    }

    public class EnglishAttribute : DisplayNameLanguageAttributeBase
    {
        public override Language Enum => Language.English;

        public EnglishAttribute(string displayName)
            : base(displayName)
        {
        }
    }

    public class JapaneseAttribute : DisplayNameLanguageAttributeBase
    {
        public override Language Enum => Language.Japanese;

        public JapaneseAttribute(string displayName)
            : base(displayName)
        {
        }
    }

    public class ChineseAttribute : DisplayNameLanguageAttributeBase
    {
        public override Language Enum => Language.Chinese;

        public ChineseAttribute(string displayName)
            : base(displayName)
        {
        }
    }
}
