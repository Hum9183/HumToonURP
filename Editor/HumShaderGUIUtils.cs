using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
// using static Unity.Rendering.Universal.ShaderUtils;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace HumToon.Editor
{
    class HumShaderGUIUtils
    {
        public static int DoPopup(MaterialEditor materialEditor, MaterialProperty property, GUIContent label, string[] displayedOptions)
        {
            int newValue = 0;
            if (property != null)
                newValue = materialEditor.PopupShaderProperty(property, label, displayedOptions);

            return newValue;
        }
        
        public static int DoPopup<T>(MaterialEditor materialEditor, MaterialProperty property, GUIContent label)
            where T: Enum
        {
            int newValue = 0;
            if (property != null)
                newValue = materialEditor.PopupShaderProperty(property, label, Enum.GetNames(typeof(T)));
            
            return newValue;
        }
        
        public static void DrawFloatToggleProperty(GUIContent styles, MaterialProperty prop, int indentLevel = 0, bool isDisabled = false)
        {
            if (prop == null)
                return;

            EditorGUI.BeginDisabledGroup(isDisabled);
            EditorGUI.indentLevel += indentLevel;
            EditorGUI.BeginChangeCheck();
            MaterialEditor.BeginProperty(prop);
            bool newValue = EditorGUILayout.Toggle(styles, prop.floatValue == 1);
            if (EditorGUI.EndChangeCheck())
                prop.floatValue = newValue ? 1.0f : 0.0f;
            MaterialEditor.EndProperty();
            EditorGUI.indentLevel -= indentLevel;
            EditorGUI.EndDisabledGroup();
        }
        
        public static bool DrawHumToggleProperty(GUIContent styles, MaterialProperty prop, int indentLevel = 0)
        {
            if (prop == null)
                return false;

            EditorGUI.indentLevel += indentLevel;
            EditorGUI.BeginChangeCheck();
            MaterialEditor.BeginProperty(prop);
            bool newValue = EditorGUILayout.Toggle(styles, (HumToggle)prop.floatValue is HumToggle.On);
            if (EditorGUI.EndChangeCheck())
                prop.floatValue = newValue ? 1.0f : 0.0f;
            MaterialEditor.EndProperty();
            EditorGUI.indentLevel -= indentLevel;

            return newValue;
        }
    }
}
