using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class ShadeStyles
    {
        // TODO: プロパティ名はベタ打ちではなく、シェーダからうまくもっていきたい。
        public static readonly GUIContent ShadeFoldout = EditorGUIUtility.TrTextContent("Shade", String.Empty);
        public static readonly GUIContent FirstShadeMap = EditorGUIUtility.TrTextContent("First Shade Map", $"{Const.Properties}_FirstShadeMap, _FirstShadeColor\n{Const.Keyword}_USE_FIRST_SHADE_MAP");
        public static readonly GUIContent FirstShadeBorderPos = EditorGUIUtility.TrTextContent("First Shade Border Pos", $"{Const.Property}_FirstShadeBorderPos");
        public static readonly GUIContent FirstShadeBorderBlur = EditorGUIUtility.TrTextContent("First Shade Border Blur", $"{Const.Property}_FirstShadeBorderBlur");
    }
}
