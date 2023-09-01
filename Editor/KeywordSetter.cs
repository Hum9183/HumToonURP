using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class KeywordSetter
    {
        public static void Set(Material material, bool isOpaque, bool alphaClip,
            bool transparentPreserveSpecular, bool transparentAlphaModulate)
        {
            // Receive Shadows
            bool receiveShadows = material.GetFloat(HumToonPropertyNames.ReceiveShadows).ToBool();
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, receiveShadows is false);

            // Alpha test
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);

            // Transparent
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT, isOpaque is false);

            // PreserveSpecular(Transparent)
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHAPREMULTIPLY_ON, transparentPreserveSpecular);

            // AlphaModulate(Transparent)
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHAMODULATE_ON, transparentAlphaModulate);
        }
    }
}
