using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace HumToon.Editor
{
    public static class HumToonGUIUtils
    {
        public static int DoPopup(MaterialEditor materialEditor, MaterialProperty matProp, GUIContent label, string[] displayedOptions)
        {
            return PopupShaderProperty(materialEditor, matProp, label, displayedOptions);
        }

        public static int DoPopup<T>(MaterialEditor materialEditor, MaterialProperty matProp, GUIContent label)
            where T: Enum
        {
            return PopupShaderProperty(materialEditor, matProp, label, Enum.GetNames(typeof(T)));
        }

        private static int PopupShaderProperty(MaterialEditor materialEditor, MaterialProperty matProp, GUIContent label, string[] displayedOptions)
        {
            int newValue = 0;
            if (matProp != null)
                newValue = materialEditor.PopupShaderProperty(matProp, label, displayedOptions);

            return newValue;
        }

        internal static void DrawFloatToggleProperty(MaterialProperty matProp, GUIContent styles, int indentLevel = 0, bool isDisabled = false)
        {
            if (matProp == null)
                return;

            EditorGUI.BeginDisabledGroup(isDisabled);
            EditorGUI.indentLevel += indentLevel;
            EditorGUI.BeginChangeCheck();
            MaterialEditor.BeginProperty(matProp);
            bool newValue = EditorGUILayout.Toggle(styles, matProp.floatValue is 1);
            if (EditorGUI.EndChangeCheck())
                matProp.floatValue = newValue ? 1.0f : 0.0f;
            MaterialEditor.EndProperty();
            EditorGUI.indentLevel -= indentLevel;
            EditorGUI.EndDisabledGroup();
        }
    }
}
