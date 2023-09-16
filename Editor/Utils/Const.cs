using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class Const
    {
        private const string HeadDecoration = "<";
        private const string TailDecoration = ">";

        public static readonly string Ln   = "\n";

        public static readonly string Description   = Decorate(nameof(Description));
        public static readonly string Property      = Decorate(nameof(Property));
        public static readonly string Properties    = Decorate(nameof(Properties));
        public static readonly string Keyword       = Decorate(nameof(Keyword));
        public static readonly string RenderTypeTag = Decorate("Tag (RenderType)");
        public static readonly string Passes        = Decorate(nameof(Passes));
        public static readonly string RenderQueue   = Decorate(nameof(RenderQueue));
        public static readonly string Other         = Decorate(nameof(Other));

        private static string Decorate(string inStr)
        {
            return $"{HeadDecoration}{inStr}{TailDecoration}";
        }
    }
}
