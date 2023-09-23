using System;
using System.Linq;
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

        public static T DoPopup<T>(MaterialEditor materialEditor, MaterialProperty matProp, GUIContent label)
            where T: Enum
        {
            var displayedOptions = Enum.GetNames(typeof(T)).ToList();
            displayedOptions = displayedOptions.Select(Utils.InsertSpaceBeforeUppercase).ToList();

            int newValue = PopupShaderProperty(materialEditor, matProp, label, displayedOptions.ToArray());

            return newValue.ToEnum<T>();
        }

        private static int PopupShaderProperty(MaterialEditor materialEditor, MaterialProperty matProp, GUIContent label, string[] displayedOptions)
        {
            int newValue = 0;
            if (matProp != null)
                newValue = materialEditor.PopupShaderProperty(matProp, label, displayedOptions);

            return newValue;
        }

        public static bool DrawFloatToggleProperty(MaterialProperty matProp, GUIContent styles)
        {
            if (matProp == null)
                throw new ArgumentNullException(nameof(matProp));

            using var changeCheckScope = new EditorGUI.ChangeCheckScope();
            MaterialEditor.BeginProperty(matProp);
            var newValue = EditorGUILayout.Toggle(styles, matProp.floatValue.IsOne());
            if (changeCheckScope.changed)
                matProp.floatValue = newValue ? 1.0f : 0.0f;
            MaterialEditor.EndProperty();

            return newValue;
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

        public static void Space(float height = 8.0f)
        {
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), new Color(1.0f,1.0f,1.0f,0.0f));
        }
    }
}
