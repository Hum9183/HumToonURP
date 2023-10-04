using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class LanguageDisplayNameAttributeBase : Attribute
    {
        public string DisplayName { get; }
        public abstract Language Enum { get; }

        protected LanguageDisplayNameAttributeBase(string displayName)
        {
            DisplayName = displayName;
        }
    }

    public class EnglishAttribute : LanguageDisplayNameAttributeBase
    {
        public override Language Enum => Language.English;

        public EnglishAttribute(string displayName)
            : base(displayName)
        {
        }
    }

    public class JapaneseAttribute : LanguageDisplayNameAttributeBase
    {
        public override Language Enum => Language.Japanese;

        public JapaneseAttribute(string displayName)
            : base(displayName)
        {
        }
    }

    public class ChineseAttribute : LanguageDisplayNameAttributeBase
    {
        public override Language Enum => Language.Chinese;

        public ChineseAttribute(string displayName)
            : base(displayName)
        {
        }
    }
}
