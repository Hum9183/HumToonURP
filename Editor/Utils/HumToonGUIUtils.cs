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

        /// <summary>
        /// Helper function to show texture and color properties.
        /// </summary>
        /// <param name="materialEditor">The material editor to use.</param>
        /// <param name="label">The label to use.</param>
        /// <param name="textureProp">The texture property.</param>
        /// <param name="colorProp">The color property.</param>
        /// <param name="hdr">Marks whether this is a HDR texture or not.</param>
        /// <returns></returns>
        public static Rect TextureColorProps(MaterialEditor materialEditor, GUIContent label, MaterialProperty textureProp, MaterialProperty colorProp, bool hdr = false)
        {
            MaterialEditor.BeginProperty(textureProp);
            if (colorProp != null)
                MaterialEditor.BeginProperty(colorProp);

            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.showMixedValue = textureProp.hasMixedValue;
            materialEditor.TexturePropertyMiniThumbnail(rect, textureProp, label.text, label.tooltip);
            EditorGUI.showMixedValue = false;

            if (colorProp != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = colorProp.hasMixedValue;
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                Rect rectAfterLabel = new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y,
                    EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
                var col = EditorGUI.ColorField(rectAfterLabel, GUIContent.none, colorProp.colorValue, true,
                    false, hdr);
                EditorGUI.indentLevel = indentLevel;
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(colorProp.displayName);
                    colorProp.colorValue = col;
                }
                EditorGUI.showMixedValue = false;
            }

            if (colorProp != null)
                MaterialEditor.EndProperty();
            MaterialEditor.EndProperty();

            return rect;
        }
    }
}
