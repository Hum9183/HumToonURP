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
using static Unity.Rendering.Universal.ShaderUtils;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace HumToon.Editor
{
    /// <summary>
    /// The base class for shader GUI in URP.
    /// </summary>
    public abstract class BaseShaderGUI : ShaderGUI
    {
        ////////////////////////////////////
        // General Functions              //
        ////////////////////////////////////
        #region GeneralFunctions


        #endregion
        ////////////////////////////////////
        // Drawing Functions              //
        ////////////////////////////////////
        #region DrawingFunctions

        // internal void DrawShaderGraphProperties(Material material, IEnumerable<MaterialProperty> properties)
        // {
        //     if (properties == null)
        //         return;
        //
        //     // NOTE : internalばかりなので一旦コメントアウト
        //     // ShaderGraphPropertyDrawers.DrawShaderGraphGUI(materialEditor, properties);
        // }

        // /// <summary>
        // /// Draws additional foldouts.
        // /// </summary>
        // /// <param name="materialScopesList"></param>
        // public virtual void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList) { }


        #endregion
        ////////////////////////////////////
        // Material Data Functions        //
        ////////////////////////////////////
        #region MaterialDataFunctions








        #endregion
        ////////////////////////////////////
        // Helper Functions               //
        ////////////////////////////////////
        #region HelperFunctions

        // /// <summary>
        // /// Helper function to draw two float variables in one lines.
        // /// </summary>
        // /// <param name="title">The title to use.</param>
        // /// <param name="prop1">The property for the first float.</param>
        // /// <param name="prop1Label">The label for the first float.</param>
        // /// <param name="prop2">The property for the second float.</param>
        // /// <param name="prop2Label">The label for the second float.</param>
        // /// <param name="materialEditor">The material editor to use.</param>
        // /// <param name="labelWidth">The width of the labels.</param>
        // public static void TwoFloatSingleLine(GUIContent title, MaterialProperty prop1, GUIContent prop1Label,
        //     MaterialProperty prop2, GUIContent prop2Label, MaterialEditor materialEditor, float labelWidth = 30f)
        // {
        //     const int kInterFieldPadding = 2;
        //
        //     MaterialEditor.BeginProperty(prop1);
        //     MaterialEditor.BeginProperty(prop2);
        //
        //     Rect rect = EditorGUILayout.GetControlRect();
        //     EditorGUI.PrefixLabel(rect, title);
        //
        //     var indent = EditorGUI.indentLevel;
        //     var preLabelWidth = EditorGUIUtility.labelWidth;
        //     EditorGUI.indentLevel = 0;
        //     EditorGUIUtility.labelWidth = labelWidth;
        //
        //     Rect propRect1 = new Rect(rect.x + preLabelWidth, rect.y,
        //         (rect.width - preLabelWidth) * 0.5f - 1, EditorGUIUtility.singleLineHeight);
        //     EditorGUI.BeginChangeCheck();
        //     EditorGUI.showMixedValue = prop1.hasMixedValue;
        //     var prop1val = EditorGUI.FloatField(propRect1, prop1Label, prop1.floatValue);
        //     if (EditorGUI.EndChangeCheck())
        //         prop1.floatValue = prop1val;
        //
        //     Rect propRect2 = new Rect(propRect1.x + propRect1.width + kInterFieldPadding, rect.y,
        //         propRect1.width, EditorGUIUtility.singleLineHeight);
        //     EditorGUI.BeginChangeCheck();
        //     EditorGUI.showMixedValue = prop2.hasMixedValue;
        //     var prop2val = EditorGUI.FloatField(propRect2, prop2Label, prop2.floatValue);
        //     if (EditorGUI.EndChangeCheck())
        //         prop2.floatValue = prop2val;
        //
        //     EditorGUI.indentLevel = indent;
        //     EditorGUIUtility.labelWidth = preLabelWidth;
        //
        //     EditorGUI.showMixedValue = false;
        //
        //     MaterialEditor.EndProperty();
        //     MaterialEditor.EndProperty();
        // }





        // // Copied from shaderGUI as it is a protected function in an abstract class, unavailable to others
        // /// <summary>
        // /// Searches and tries to find a property in an array of properties.
        // /// </summary>
        // /// <param name="propertyName">The property to find.</param>
        // /// <param name="properties">Array of properties to search in.</param>
        // /// <returns>A MaterialProperty instance for the property.</returns>
        // public new static MaterialProperty FindProperty(string propertyName, MaterialProperty[] properties)
        // {
        //     return FindProperty(propertyName, properties, true);
        // }



        #endregion
    }
}
