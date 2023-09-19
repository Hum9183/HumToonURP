using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using P = HumToon.Editor.SurfaceOptionsPropertyContainer;

namespace HumToon.Editor
{
    public class SurfaceOptionsValidator : IHeaderScopeValidator
    {
        private static readonly int IDSurfaceType          = Shader.PropertyToID($"{nameof(P.SurfaceType).Prefix()}");
        private static readonly int IDAlphaClip            = Shader.PropertyToID($"{nameof(P.AlphaClip).Prefix()}");
        private static readonly int IDTransparentBlendMode = Shader.PropertyToID($"{nameof(P.BlendMode).Prefix()}");
        private static readonly int IDReceiveShadows       = Shader.PropertyToID($"{nameof(P.ReceiveShadows).Prefix()}");
        private static readonly int IDCullMode             = Shader.PropertyToID($"{nameof(P.CullMode).Prefix()}");
        private static readonly int IDAlphaToMask          = Shader.PropertyToID(HumToonPropertyNames.AlphaToMask);
        private static readonly int IDZWrite               = Shader.PropertyToID(HumToonPropertyNames.ZWrite);
        private static readonly int IDQueueOffset          = Shader.PropertyToID(HumToonPropertyNames.QueueOffset);

        public void Validate(Material material)
        {
            var isOpaque = (SurfaceType)material.GetFloat(IDSurfaceType) is SurfaceType.Opaque;
            var alphaClip = material.GetFloat(IDAlphaClip).ToBool();
            var transparentBlendMode = (TransparentBlendMode)material.GetFloat(IDTransparentBlendMode);
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
            bool receiveShadows = material.GetFloat(IDReceiveShadows).ToBool();
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
            material.SetOverrideTag(RenderTypeTagNames.RenderType, string.Empty); // TODO: 必要ないかも

            if (isOpaque)
            {
                material.SetOverrideTag(RenderTypeTagNames.RenderType, alphaClip ? RenderTypeTagNames.TransparentCutout : RenderTypeTagNames.Opaque);
            }
            else
            {
                material.SetOverrideTag(RenderTypeTagNames.RenderType, RenderTypeTagNames.Transparent);
            }
        }

        private void SetPass(Material material, bool isOpaque)
        {
            // Transparent
            material.SetShaderPassEnabled(PassNames.ShadowCaster, isOpaque);

            // Depth
            material.SetShaderPassEnabled(PassNames.DepthOnly, isOpaque);
        }

        private void SetFloat(Material material, bool isOpaque, bool alphaClip)
        {
            material.SetFloat(IDAlphaToMask, alphaClip.ToFloat());

            material.SetFloat(IDZWrite, isOpaque.ToFloat());
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

            renderQueue += (int)material.GetFloat(IDQueueOffset);

            if (material.renderQueue != renderQueue)
                material.renderQueue = renderQueue;
        }

        private void SetOthers(Material material)
        {
            // Setup double sided GI based on Cull state
            material.doubleSidedGI = (RenderFace)material.GetFloat(IDCullMode) != RenderFace.Front;
        }
    }
}
