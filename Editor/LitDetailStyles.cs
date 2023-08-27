using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public static class LitDetailStyles
    {
        public static readonly GUIContent detailInputs = EditorGUIUtility.TrTextContent("Detail Inputs",
            "These settings define the surface details by tiling and overlaying additional maps on the surface.");

        public static readonly GUIContent detailMaskText = EditorGUIUtility.TrTextContent("Mask",
            "Select a mask for the Detail map. The mask uses the alpha channel of the selected texture. The Tiling and Offset settings have no effect on the mask.");

        public static readonly GUIContent detailAlbedoMapText = EditorGUIUtility.TrTextContent("Base Map",
            "Select the surface detail texture.The alpha of your texture determines surface hue and intensity.");

        public static readonly GUIContent detailNormalMapText = EditorGUIUtility.TrTextContent("Normal Map",
            "Designates a Normal Map to create the illusion of bumps and dents in the details of this Material's surface.");

        public static readonly GUIContent detailAlbedoMapScaleInfo = EditorGUIUtility.TrTextContent("Setting the scaling factor to a value other than 1 results in a less performant shader variant.");
        public static readonly GUIContent detailAlbedoMapFormatError = EditorGUIUtility.TrTextContent("This texture is not in linear space.");
    }
}
