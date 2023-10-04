using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using C = Hum.HumToon.Editor.Utils.Const;
using L = Hum.HumToon.Editor.Language.LanguageSelector;
using P = Hum.HumToon.Editor.HeaderScopes.SurfaceOptions.SurfaceOptionsPropertiesContainer;

namespace Hum.HumToon.Editor.HeaderScopes.SurfaceOptions
{
    /// <summary>
    /// Container for the text and tooltips used to display the shader.
    /// </summary>
    public static class SurfaceOptionsStyles
    {
        public static GUIContent SurfaceOptionsFoldout =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Surface Options", "サーフェス設定", "表面选项" })}",
                tooltip: $"{C.Description}{C.Ln}" +
                         $"Controls how URP Renders the material on screen.");

        public static GUIContent SurfaceType =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Surface Type", "サーフェスタイプ", "表面类型" })}",
                tooltip: $"{C.Description}{C.Ln}" +
                         $"Select a surface type for your texture. Choose between Opaque or Transparent.{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.Properties}{C.Ln}" +
                         $"{nameof(P.SurfaceType).Prefix()}{C.Ln}" +
                         $"{HumToonPropertyNames.ZWrite}{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.Keyword}{C.Ln}" +
                         $"{ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT}{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.RenderTypeTag}{C.Ln}" +
                         $"{RenderTypeTagNames.Opaque} or {RenderTypeTagNames.Transparent}{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.Passes}{C.Ln}" +
                         $"{PassNames.ShadowCaster}{C.Ln}" +
                         $"{PassNames.DepthOnly}{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.RenderQueue}{C.Ln}" +
                         $"{RenderQueue.Geometry} or {RenderQueue.Transparent}");

        public static GUIContent TransparentBlendMode =>
            EditorGUIUtility.TrTextContent(
                text: $"{L.Select(new string[] { "Blending Mode", "合成モード", "混合模式" })}",
                tooltip: $"{C.Description}{C.Ln}" +
                         $"Controls how the color of the Transparent surface blends with the Material color in the background.{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.Property}{C.Ln}" +
                         $"{nameof(P.BlendMode).Prefix()}{C.Ln}" +
                         $"{C.Ln}" +
                         $"{C.Keyword}{C.Ln}" +
                         $"{ShaderKeywordStrings._ALPHAMODULATE_ON}");

        public static readonly GUIContent RenderFace = EditorGUIUtility.TrTextContent(
            text: "Render Face",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Specifies which faces to cull from your geometry. " +
                     $"Front culls front faces. Back culls backfaces. " +
                     $"None means that both sides are rendered.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Property}{C.Ln}" +
                     $"{nameof(P.CullMode).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Other}{C.Ln}" +
                     $"material.doubleSidedGI");

        public static readonly GUIContent AlphaClip = EditorGUIUtility.TrTextContent(
            text: "Alpha Clipping",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Makes your Material act like a Cutout shader. " +
                     $"Use this to create a transparent effect with hard edges between opaque and transparent areas.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Properties}{C.Ln}" +
                     $"{nameof(P.AlphaClip).Prefix()}{C.Ln}" +
                     $"{HumToonPropertyNames.AlphaToMask}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShaderKeywordStrings._ALPHATEST_ON}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.RenderTypeTag}{C.Ln}" +
                     $"{RenderTypeTagNames.TransparentCutout}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.RenderQueue}{C.Ln}" +
                     $"{RenderQueue.AlphaTest}");

        public static readonly GUIContent Cutoff = EditorGUIUtility.TrTextContent(
            text: "Threshold",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"Sets where the Alpha Clipping starts. " +
                     $"The higher the value is, the brighter the  effect is when clipping starts.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Property}{C.Ln}" +
                     $"{nameof(P.Cutoff).Prefix()}");

        public static readonly GUIContent ReceiveShadow = EditorGUIUtility.TrTextContent(
            text: "Receive Shadows",
            tooltip: $"{C.Description}{C.Ln}" +
                     $"When enabled, other GameObjects can cast shadows onto this GameObject.{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Property}{C.Ln}" +
                     $"{nameof(P.ReceiveShadows).Prefix()}{C.Ln}" +
                     $"{C.Ln}" +
                     $"{C.Keyword}{C.Ln}" +
                     $"{ShaderKeywordStrings._RECEIVE_SHADOWS_OFF}");
    }
}
