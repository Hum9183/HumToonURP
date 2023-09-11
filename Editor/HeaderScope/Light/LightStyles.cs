using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class LightStyles
    {
        public static readonly GUIContent LightFoldout = EditorGUIUtility.TrTextContent("Light", String.Empty);
        public static readonly GUIContent MainLightColorWeight = EditorGUIUtility.TrTextContent("Main Light Color Weight", $"{Const.Property}_MainLightColorWeight");
        public static readonly GUIContent AdditionalLightsColorWeight = EditorGUIUtility.TrTextContent("Additional Lights Color Weight", $"{Const.Property}_AdditionalLightColorWeight");
    }
}
