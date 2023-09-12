using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class LightStyles
    {
        public static readonly GUIContent LightFoldout = EditorGUIUtility.TrTextContent("Light", String.Empty);
        public static readonly GUIContent MainLightColorWeight = EditorGUIUtility.TrTextContent("Main Light Color Weight", $"{Const.Property}_MainLightColorWeight");
        public static readonly GUIContent MainLightUpperLimit = EditorGUIUtility.TrTextContent("Main Light Upper Limit", $"{Const.Property}_UseMainLightUpperLimit, _MainLightUpperLimit");
        public static readonly GUIContent MainLightLowerLimit = EditorGUIUtility.TrTextContent("Main Light Lower Limit", $"{Const.Property}_UseMainLightLowerLimit, _MainLightLowerLimit");
        public static readonly GUIContent AdditionalLightsColorWeight = EditorGUIUtility.TrTextContent("Additional Lights Color Weight", $"{Const.Property}_AdditionalLightColorWeight");
    }
}
