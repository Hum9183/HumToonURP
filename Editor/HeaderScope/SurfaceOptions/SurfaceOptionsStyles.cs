using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class SurfaceOptionsStyles
    {
        // Categories
        /// <summary>
        /// The text and tooltip for the surface options GUI.
        /// </summary>
        public static readonly GUIContent SurfaceOptionsFoldout = EditorGUIUtility.TrTextContent("Surface Options",
            "Controls how URP Renders the material on screen.");

        /// <summary>
        /// The text and tooltip for the Surface Type GUI.
        /// </summary>
        public static readonly GUIContent SurfaceType = EditorGUIUtility.TrTextContent("Surface Type",
            "Select a surface type for your texture. Choose between Opaque or Transparent.");

        /// <summary>
        /// The text and tooltip for the blending mode GUI.
        /// </summary>
        public static readonly GUIContent BlendingMode = EditorGUIUtility.TrTextContent("Blending Mode",
            "Controls how the color of the Transparent surface blends with the Material color in the background.");


        /// <summary>
        /// The text and tooltip for the render face GUI.
        /// </summary>
        public static readonly GUIContent RenderFace = EditorGUIUtility.TrTextContent("Render Face",
            "Specifies which faces to cull from your geometry. Front culls front faces. Back culls backfaces. None means that both sides are rendered.");

        /// <summary>
        /// The text and tooltip for the alpha clipping GUI.
        /// </summary>
        public static readonly GUIContent AlphaClip = EditorGUIUtility.TrTextContent("Alpha Clipping",
            "Makes your Material act like a Cutout shader. Use this to create a transparent effect with hard edges between opaque and transparent areas.");

        /// <summary>
        /// The text and tooltip for the alpha clipping threshold GUI.
        /// </summary>
        public static readonly GUIContent Cutoff = EditorGUIUtility.TrTextContent("Threshold",
            "Sets where the Alpha Clipping starts. The higher the value is, the brighter the  effect is when clipping starts.");

        /// <summary>
        /// The text and tooltip for the receive shadows GUI.
        /// </summary>
        public static readonly GUIContent ReceiveShadow = EditorGUIUtility.TrTextContent("Receive Shadows",
            "When enabled, other GameObjects can cast shadows onto this GameObject.");

    }
}
