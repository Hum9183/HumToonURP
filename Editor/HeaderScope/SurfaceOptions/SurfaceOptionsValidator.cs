using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public class SurfaceOptionsValidator : IHeaderScopeValidator
    {
        private const string RenderType = "RenderType";

        public void Validate(Material material)
        {
            var isOpaque = Utils.IsOpaque(material);
            var alphaClip = material.GetFloat(HumToonPropertyNames.AlphaClip).ToBool();
            var transparentBlendMode = (TransparentBlendMode)material.GetFloat(HumToonPropertyNames.BlendMode);
            var transparentPreserveSpecular = isOpaque is false && Utils.GetPreserveSpecular(material, transparentBlendMode);
            var transparentAlphaModulate = isOpaque is false && transparentBlendMode is TransparentBlendMode.Multiply;

            SetKeywords(material, isOpaque, alphaClip, transparentPreserveSpecular, transparentAlphaModulate);
            SetTags(material, isOpaque, alphaClip);
            SetPass(material, isOpaque);
            SetFloat(material, isOpaque, alphaClip);
            BlendSetter.Set(material, isOpaque, transparentBlendMode, transparentPreserveSpecular);
            SetRenderQueue(material, isOpaque, alphaClip);
            SetOthers(material);
        }

        private void SetKeywords(Material material, bool isOpaque, bool alphaClip,
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

        private void SetTags(Material material, bool isOpaque, bool alphaClip)
        {
            // Clear override tag
            material.SetOverrideTag(RenderType, string.Empty); // TODO: 必要ないかも

            if (isOpaque)
            {
                material.SetOverrideTag(RenderType, alphaClip ? "TransparentCutout" : "Opaque");
            }
            else
            {
                material.SetOverrideTag(RenderType, "Transparent");
            }
        }

        private void SetPass(Material material, bool isOpaque)
        {
            // Transparent
            material.SetShaderPassEnabled("ShadowCaster", isOpaque);

            // Depth
            material.SetShaderPassEnabled("DepthOnly", isOpaque);
        }

        private void SetFloat(Material material, bool isOpaque, bool alphaClip)
        {
            material.SetFloat(HumToonPropertyNames.AlphaToMask, alphaClip.ToFloat());

            material.SetFloat(HumToonPropertyNames.ZWrite, isOpaque.ToFloat());
        }

        private void SetRenderQueue(Material material, bool isOpaque, bool alphaClip)
        {
            int renderQueue;

            if (isOpaque)
            {
                renderQueue = alphaClip ? (int)RenderQueue.AlphaTest : (int)RenderQueue.Geometry;
            }
            else
            {
                renderQueue = (int)RenderQueue.Transparent;
            }

            renderQueue += (int)material.GetFloat(HumToonPropertyNames.QueueOffset);

            if (material.renderQueue != renderQueue)
                material.renderQueue = renderQueue;
        }

        private void SetOthers(Material material)
        {
            // Setup double sided GI based on Cull state
            material.doubleSidedGI = (RenderFace)material.GetFloat(HumToonPropertyNames.CullMode) != RenderFace.Front;
        }
    }
}
