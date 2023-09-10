using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace HumToon.Editor
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class SurfaceInputsStyles
    {
        /// <summary>
        /// The text and tooltip for the surface inputs GUI.
        /// </summary>
        public static readonly GUIContent SurfaceInputsFoldout = EditorGUIUtility.TrTextContent("Surface Inputs",
            "These settings describe the look and feel of the surface itself.");

        /// <summary>
        /// The text and tooltip for the base map GUI.
        /// </summary>
        public static readonly GUIContent BaseMap = EditorGUIUtility.TrTextContent("Base Map",
            "Specifies the base Material and/or Color of the surface. If you’ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture’s alpha channel or color.");

        /// <summary>
        /// The text and tooltip for the normal map GUI.
        /// </summary>
        public static readonly GUIContent NormalMap =
            EditorGUIUtility.TrTextContent("Normal Map", "Designates a Normal Map to create the illusion of bumps and dents on this Material's surface.");

        /// <summary>
        /// The text and tooltip for the bump scale not supported GUI.
        /// </summary>
        public static readonly GUIContent BumpScaleNotSupported =
            EditorGUIUtility.TrTextContent("Bump scale is not supported on mobile platforms");

        /// <summary>
        /// The text and tooltip for the normals fix now GUI.
        /// </summary>
        public static readonly GUIContent FixNormalNow = EditorGUIUtility.TrTextContent("Fix now",
            "Converts the assigned texture to be a normal map format.");
    }
}
