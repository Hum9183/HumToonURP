using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Hum.HumToon.Editor.Utils
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
            // TODO: showMixedValue
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

        public static Rect GetControlRectForSingleLine() => EditorGUILayout.GetControlRect(true, 20f, EditorStyles.layerMaskField);

        public static (bool, float) FloatToggleAndRangePropertiesSingleLine(
            MaterialEditor materialEditor,
            MaterialProperty floatToggleProp,
            MaterialProperty rangeProp,
            GUIContent label)
        {
            bool floatToggleNewValue;
            float rangeNewValue;

            Rect rectForSingleLine = GetControlRectForSingleLine();

            MaterialEditor.BeginProperty(rectForSingleLine, floatToggleProp);
            MaterialEditor.BeginProperty(rectForSingleLine, rangeProp);

            floatToggleNewValue = FloatToggleProperty();
            rangeNewValue = RangeProperty(floatToggleNewValue);

            MaterialEditor.EndProperty();
            MaterialEditor.EndProperty();

            return (floatToggleNewValue, rangeNewValue);

            bool FloatToggleProperty()
            {
                bool floatToggleNewValueInternal;

                using var changeCheckScope = new EditorGUI.ChangeCheckScope();
                floatToggleNewValueInternal = EditorGUI.Toggle(rectForSingleLine, label, floatToggleProp.floatValue.IsOne());
                if (changeCheckScope.changed)
                    floatToggleProp.floatValue = floatToggleNewValueInternal ? 1.0f : 0.0f;

                return floatToggleNewValueInternal;
            }

            float RangeProperty(bool floatToggleNewValueInternal)
            {
                float rangeNewValueInternal;

                using (new EditorGUI.DisabledScope(!floatToggleNewValueInternal))
                {
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    materialEditor.ShaderProperty(MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rectForSingleLine), rangeProp, string.Empty);
                    rangeNewValueInternal = rangeProp.floatValue;

                    EditorGUI.indentLevel = indentLevel;
                }

                return rangeNewValueInternal;
            }
        }

        public static (bool, float) TextureAndRangePropertiesSingleLine(
            MaterialEditor materialEditor,
            MaterialProperty textureProp,
            MaterialProperty rangeProp,
            GUIContent label)
        {
            bool existsTexture;
            float rangeNewValue;

            Rect rectForSingleLine = GetControlRectForSingleLine();

            MaterialEditor.BeginProperty(rectForSingleLine, textureProp);
            MaterialEditor.BeginProperty(rectForSingleLine, rangeProp);

            existsTexture = TextureProperty();
            rangeNewValue = RangeProperty(existsTexture);

            MaterialEditor.EndProperty();
            MaterialEditor.EndProperty();

            return (existsTexture, rangeNewValue);

            bool TextureProperty()
            {
                bool existsTextureInternal;

                materialEditor.TexturePropertyMiniThumbnail(rectForSingleLine, textureProp, label.text, label.tooltip);
                existsTextureInternal = textureProp.textureValue;

                return existsTextureInternal;
            }

            float RangeProperty(bool existsTextureInternal)
            {
                float rangeNewValueInternal;

                using (new EditorGUI.DisabledScope(!existsTextureInternal))
                {
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    materialEditor.ShaderProperty(MaterialEditor.GetRectAfterLabelWidth(rectForSingleLine), rangeProp, string.Empty);
                    rangeNewValueInternal = rangeProp.floatValue;

                    EditorGUI.indentLevel = indentLevel;
                }

                return rangeNewValueInternal;
            }
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
