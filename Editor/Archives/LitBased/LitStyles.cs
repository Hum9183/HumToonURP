using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class LitStyles
    {
        /// <summary>
        /// The text and tooltip for the workflow Mode GUI.
        /// </summary>
        public static readonly GUIContent WorkflowMode = EditorGUIUtility.TrTextContent("Workflow Mode",
            "Select a workflow that fits your textures. Choose between Metallic or Specular.");

        /// <summary>
        /// The text and tooltip for the specular Map GUI.
        /// </summary>
        public static readonly GUIContent SpecularMap =
            EditorGUIUtility.TrTextContent("Specular Map", "Designates a Specular Map and specular color determining the apperance of reflections on this Material's surface.");

        /// <summary>
        /// The text and tooltip for the metallic Map GUI.
        /// </summary>
        public static readonly GUIContent MetallicMap =
            EditorGUIUtility.TrTextContent("Metallic Map", "Sets and configures the map for the Metallic workflow.");

        /// <summary>
        /// The text and tooltip for the smoothness GUI.
        /// </summary>
        public static readonly GUIContent Smoothness = EditorGUIUtility.TrTextContent("Smoothness",
            "Controls the spread of highlights and reflections on the surface.");

        /// <summary>
        /// The text and tooltip for the smoothness source GUI.
        /// </summary>
        public static readonly GUIContent SmoothnessTextureChannel =
            EditorGUIUtility.TrTextContent("Source",
                "Specifies where to sample a smoothness map from. By default, uses the alpha channel for your map.");

        /// <summary>
        /// The text and tooltip for the specular Highlights GUI.
        /// </summary>
        public static readonly GUIContent Highlights = EditorGUIUtility.TrTextContent("Specular Highlights",
            "When enabled, the Material reflects the shine from direct lighting.");

        /// <summary>
        /// The text and tooltip for the environment Reflections GUI.
        /// </summary>
        public static readonly GUIContent Reflections =
            EditorGUIUtility.TrTextContent("Environment Reflections",
                "When enabled, the Material samples reflections from the nearest Reflection Probes or Lighting Probe.");

        /// <summary>
        /// The text and tooltip for the height map GUI.
        /// </summary>
        public static readonly GUIContent HeightMap = EditorGUIUtility.TrTextContent("Height Map",
            "Defines a Height Map that will drive a parallax effect in the shader making the surface seem displaced.");

        /// <summary>
        /// The text and tooltip for the occlusion map GUI.
        /// </summary>
        public static readonly GUIContent Occlusion = EditorGUIUtility.TrTextContent("Occlusion Map",
            "Sets an occlusion map to simulate shadowing from ambient lighting.");

        /// <summary>
        /// The names for smoothness alpha options available for metallic workflow.
        /// </summary>
        public static readonly string[] MetallicSmoothnessChannelNames = { "Metallic Alpha", "Albedo Alpha" };

        /// <summary>
        /// The names for smoothness alpha options available for specular workflow.
        /// </summary>
        public static readonly string[] SpecularSmoothnessChannelNames = { "Specular Alpha", "Albedo Alpha" };

        /// <summary>
        /// The text and tooltip for the enabling/disabling clear coat GUI.
        /// </summary>
        public static readonly GUIContent ClearCoat = EditorGUIUtility.TrTextContent("Clear Coat",
            "A multi-layer material feature which simulates a thin layer of coating on top of the surface material." +
            "\nPerformance cost is considerable as the specular component is evaluated twice, once per layer.");

        /// <summary>
        /// The text and tooltip for the clear coat Mask GUI.
        /// </summary>
        public static readonly GUIContent ClearCoatMask = EditorGUIUtility.TrTextContent("Mask",
            "Specifies the amount of the coat blending." +
            "\nActs as a multiplier of the clear coat map mask value or as a direct mask value if no map is specified." +
            "\nThe map specifies clear coat mask in the red channel and clear coat smoothness in the green channel.");

        /// <summary>
        /// The text and tooltip for the clear coat smoothness GUI.
        /// </summary>
        public static readonly GUIContent ClearCoatSmoothness = EditorGUIUtility.TrTextContent("Smoothness",
            "Specifies the smoothness of the coating." +
            "\nActs as a multiplier of the clear coat map smoothness value or as a direct smoothness value if no map is specified.");
    }
}
