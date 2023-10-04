using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hum.HumToon.Editor.Language
{
    public enum Language
    {
        [English("English")]
        [Japanese("英語")]
        [Chinese("英语")]
        English,

        [English("Japanese")]
        [Japanese("日本語")]
        [Chinese("日语")]
        Japanese,

        [English("Chinese")]
        [Japanese("中国語")]
        [Chinese("中文")]
        Chinese
    }
}
