using System;
using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class MatCapStyles
    {
        // TODO: プロパティ名はベタ打ちではなく、シェーダからうまくもっていきたい。
        public static readonly GUIContent MatCapFoldout = EditorGUIUtility.TrTextContent("MatCap", String.Empty);
        public static readonly GUIContent UseMatCap = EditorGUIUtility.TrTextContent("Use Mat Cap", $"{Const.Property}_UseMatCap\n{Const.Keyword}_USE_MAT_CAP");
        public static readonly GUIContent MatCapMap = EditorGUIUtility.TrTextContent("Mat Cap Map", $"{Const.Properties}_MatCapMap, _MatCapColor");
        public static readonly GUIContent MatCapMainLightEffectiveness = EditorGUIUtility.TrTextContent("Main Light Effectiveness", $"{Const.Property}_MatCapMainLightEffectiveness");
    }
}
