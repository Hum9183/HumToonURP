using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class ShadeStyles
    {
        public static readonly GUIContent ShadeFoldout = EditorGUIUtility.TrTextContent("Shade", String.Empty);
        public static readonly GUIContent FirstShadeMap = EditorGUIUtility.TrTextContent("First Shade Map", "_FirstShadeColor & _FirstShadeMap");
        public static readonly GUIContent FirstShadeBorderPos = EditorGUIUtility.TrTextContent("First Shade Border Pos", "_FirstShadeBorderPos");
        public static readonly GUIContent FirstShadeBorderBlur = EditorGUIUtility.TrTextContent("First Shade Border Blur", "_FirstShadeBorderBlur");
    }
}
