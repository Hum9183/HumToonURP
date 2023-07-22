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
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    internal class Styles
    {
        /// <summary>
        /// The names for options available in the SurfaceType enum.
        /// </summary>
        public static readonly string[] surfaceTypeNames = Enum.GetNames(typeof(SurfaceType));

        /// <summary>
        /// The names for options available in the BlendMode enum.
        /// </summary>
        public static readonly string[] blendModeNames = Enum.GetNames(typeof(BlendMode));

        /// <summary>
        /// The names for options available in the RenderFace enum.
        /// </summary>
        public static readonly string[] renderFaceNames = Enum.GetNames(typeof(RenderFace));

        /// <summary>
        /// The names for options available in the ZWriteControl enum.
        /// </summary>
        public static readonly string[] zwriteNames = Enum.GetNames(typeof(ZWriteControl));

        /// <summary>
        /// The names for options available in the QueueControl enum.
        /// </summary>
        public static readonly string[] queueControlNames = Enum.GetNames(typeof(QueueControl));

        /// <summary>
        /// The values for options available in the ZTestMode enum.
        /// </summary>
        // Skipping the first entry for ztest (ZTestMode.Disabled is not a valid value)
        public static readonly int[] ztestValues = ((int[])Enum.GetValues(typeof(ZTestMode))).Skip(1).ToArray();

        /// <summary>
        /// The names for options available in the ZTestMode enum.
        /// </summary>
        // Skipping the first entry for ztest (ZTestMode.Disabled is not a valid value)
        public static readonly string[] ztestNames = Enum.GetNames(typeof(ZTestMode)).Skip(1).ToArray();

        // Categories
        /// <summary>
        /// The text and tooltip for the surface options GUI.
        /// </summary>
        public static readonly GUIContent SurfaceOptions =
            EditorGUIUtility.TrTextContent("Surface Options", "Controls how URP Renders the material on screen.");

        /// <summary>
        /// The text and tooltip for the surface inputs GUI.
        /// </summary>
        public static readonly GUIContent SurfaceInputs = EditorGUIUtility.TrTextContent("Surface Inputs",
            "These settings describe the look and feel of the surface itself.");

        /// <summary>
        /// The text and tooltip for the advanced options GUI.
        /// </summary>
        public static readonly GUIContent AdvancedLabel = EditorGUIUtility.TrTextContent("Advanced Options",
            "These settings affect behind-the-scenes rendering and underlying calculations.");

        /// <summary>
        /// The text and tooltip for the Surface Type GUI.
        /// </summary>
        public static readonly GUIContent surfaceType = EditorGUIUtility.TrTextContent("Surface Type",
            "Select a surface type for your texture. Choose between Opaque or Transparent.");

        /// <summary>
        /// The text and tooltip for the blending mode GUI.
        /// </summary>
        public static readonly GUIContent blendingMode = EditorGUIUtility.TrTextContent("Blending Mode",
            "Controls how the color of the Transparent surface blends with the Material color in the background.");

        /// <summary>
        /// The text and tooltip for the preserve specular lighting GUI.
        /// </summary>
        public static readonly GUIContent preserveSpecularText = EditorGUIUtility.TrTextContent("Preserve Specular Lighting",
            "Preserves specular lighting intensity and size by not applying transparent alpha to the specular light contribution.");

        /// <summary>
        /// The text and tooltip for the render face GUI.
        /// </summary>
        public static readonly GUIContent cullingText = EditorGUIUtility.TrTextContent("Render Face",
            "Specifies which faces to cull from your geometry. Front culls front faces. Back culls backfaces. None means that both sides are rendered.");

        /// <summary>
        /// The text and tooltip for the depth write GUI.
        /// </summary>
        public static readonly GUIContent zwriteText = EditorGUIUtility.TrTextContent("Depth Write",
            "Controls whether the shader writes depth.  Auto will write only when the shader is opaque.");

        /// <summary>
        /// The text and tooltip for the depth test GUI.
        /// </summary>
        public static readonly GUIContent ztestText = EditorGUIUtility.TrTextContent("Depth Test",
            "Specifies the depth test mode.  The default is LEqual.");

        /// <summary>
        /// The text and tooltip for the alpha clipping GUI.
        /// </summary>
        public static readonly GUIContent alphaClipText = EditorGUIUtility.TrTextContent("Alpha Clipping",
            "Makes your Material act like a Cutout shader. Use this to create a transparent effect with hard edges between opaque and transparent areas.");

        /// <summary>
        /// The text and tooltip for the alpha clipping threshold GUI.
        /// </summary>
        public static readonly GUIContent alphaClipThresholdText = EditorGUIUtility.TrTextContent("Threshold",
            "Sets where the Alpha Clipping starts. The higher the value is, the brighter the  effect is when clipping starts.");

        /// <summary>
        /// The text and tooltip for the cast shadows GUI.
        /// </summary>
        public static readonly GUIContent castShadowText = EditorGUIUtility.TrTextContent("Cast Shadows",
            "When enabled, this GameObject will cast shadows onto any geometry that can receive them.");

        /// <summary>
        /// The text and tooltip for the receive shadows GUI.
        /// </summary>
        public static readonly GUIContent receiveShadowText = EditorGUIUtility.TrTextContent("Receive Shadows",
            "When enabled, other GameObjects can cast shadows onto this GameObject.");

        /// <summary>
        /// The text and tooltip for the base map GUI.
        /// </summary>
        public static readonly GUIContent baseMap = EditorGUIUtility.TrTextContent("Base Map",
            "Specifies the base Material and/or Color of the surface. If you’ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture’s alpha channel or color.");

        /// <summary>
        /// The text and tooltip for the emission map GUI.
        /// </summary>
        public static readonly GUIContent emissionMap = EditorGUIUtility.TrTextContent("Emission Map",
            "Determines the color and intensity of light that the surface of the material emits.");

        /// <summary>
        /// The text and tooltip for the normal map GUI.
        /// </summary>
        public static readonly GUIContent normalMapText =
            EditorGUIUtility.TrTextContent("Normal Map", "Designates a Normal Map to create the illusion of bumps and dents on this Material's surface.");

        /// <summary>
        /// The text and tooltip for the bump scale not supported GUI.
        /// </summary>
        public static readonly GUIContent bumpScaleNotSupported =
            EditorGUIUtility.TrTextContent("Bump scale is not supported on mobile platforms");

        /// <summary>
        /// The text and tooltip for the normals fix now GUI.
        /// </summary>
        public static readonly GUIContent fixNormalNow = EditorGUIUtility.TrTextContent("Fix now",
            "Converts the assigned texture to be a normal map format.");

        /// <summary>
        /// The text and tooltip for the sorting priority GUI.
        /// </summary>
        public static readonly GUIContent queueSlider = EditorGUIUtility.TrTextContent("Sorting Priority",
            "Determines the chronological rendering order for a Material. Materials with lower value are rendered first.");

        /// <summary>
        /// The text and tooltip for the queue control GUI.
        /// </summary>
        public static readonly GUIContent queueControl = EditorGUIUtility.TrTextContent("Queue Control",
            "Controls whether render queue is automatically set based on material surface type, or explicitly set by the user.");

        /// <summary>
        /// The text and tooltip for the help reference GUI.
        /// </summary>
        public static readonly GUIContent documentationIcon = EditorGUIUtility.TrIconContent("_Help", $"Open Reference for URP Shaders.");
    }
}
