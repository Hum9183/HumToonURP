using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class MaterialBlendModeSetter
    {
        private const string RenderType = "RenderType";

        /// <summary>
        /// Setup blending
        /// </summary>
        public static void Setup(Material material, out int automaticRenderQueue)
        {
            if (material is null)
                throw new ArgumentNullException(nameof(material));

            // alpha test
            bool alphaClip = material.GetFloat(HumToonPropertyNames.AlphaClip).ToBool();
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);

            // transparent
            SurfaceType surfaceType = (SurfaceType)material.GetFloat(HumToonPropertyNames.SurfaceType);
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT, surfaceType is SurfaceType.Transparent);

            // clear override tag
            material.SetOverrideTag(RenderType, string.Empty);

            int renderQueue;
            if (surfaceType is SurfaceType.Opaque)
            {
                renderQueue = OpaqueType(material, alphaClip);
            }
            else
            {
                renderQueue = TransparentType(material);
            }

            // must always apply queue offset, even if not set to material control
            renderQueue += (int)material.GetFloat(HumToonPropertyNames.QueueOffset);

            automaticRenderQueue = renderQueue;
        }

        private static int OpaqueType(Material material, bool alphaClip)
        {
            int renderQueue;
            if (alphaClip)
            {
                renderQueue = (int)RenderQueue.AlphaTest;
                material.SetOverrideTag(RenderType, "TransparentCutout");
            }
            else
            {
                renderQueue = (int)RenderQueue.Geometry;
                material.SetOverrideTag(RenderType, "Opaque");
            }

            SetMaterialSrcDstBlendProperties(material, BlendMode.One, BlendMode.Zero);

            material.DisableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
            material.DisableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
            material.DisableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);

            return renderQueue;
        }

        private static int TransparentType(Material material)
        {
            TransparentBlendMode transparentBlendMode = (TransparentBlendMode)material.GetFloat(HumToonPropertyNames.BlendMode);

            // Clear blend keyword state.
            material.DisableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
            material.DisableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);

            var (srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA) = SwitchBlendMode(material, transparentBlendMode);

            // Lift alpha multiply from ROP to shader by setting pre-multiplied _SrcBlend mode.
            // The intent is to do different blending for diffuse and specular in shader.
            // ref: http://advances.realtimerendering.com/other/2016/naughty_dog/NaughtyDog_TechArt_Final.pdf
            bool preserveSpecular = (material.HasProperty(HumToonPropertyNames.BlendModePreserveSpecular) &&
                                     material.GetFloat(HumToonPropertyNames.BlendModePreserveSpecular) > 0) &&
                                    transparentBlendMode != TransparentBlendMode.Multiply && transparentBlendMode != TransparentBlendMode.Premultiply;
            if (preserveSpecular)
            {
                srcBlendRGB = BlendMode.One;
                material.EnableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
            }

            SetMaterialSrcDstBlendProperties(material, srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA);

            // General Transparent Material Settings
            material.SetOverrideTag(RenderType, "Transparent");
            material.EnableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
            int renderQueue = (int)RenderQueue.Transparent;
            material.SetFloat(HumToonPropertyNames.AlphaToMask, 0.0f); // NOTE: デフォルト値のままなので、要確認

            material.SetFloat(HumToonPropertyNames.ZWrite, 0.0f);
            material.SetShaderPassEnabled("DepthOnly", false);

            return renderQueue;
        }

        private static (BlendMode srcBlendRGB, BlendMode dstBlendRGB, BlendMode srcBlendA, BlendMode dstBlendA)
            SwitchBlendMode(Material material, TransparentBlendMode transparentBlendMode)
        {
            var srcBlendRGB = BlendMode.One;
            var dstBlendRGB = BlendMode.OneMinusSrcAlpha;
            var srcBlendA   = BlendMode.One;
            var dstBlendA   = BlendMode.OneMinusSrcAlpha;

            // Specific Transparent Mode Settings
            switch (transparentBlendMode)
            {
                // srcRGB * srcAlpha + dstRGB * (1 - srcAlpha)
                // preserve spec:
                // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                case TransparentBlendMode.Alpha:
                    srcBlendRGB = BlendMode.SrcAlpha;
                    dstBlendRGB = BlendMode.OneMinusSrcAlpha;
                    srcBlendA   = BlendMode.One;
                    dstBlendA   = dstBlendRGB;
                    break;

                // srcRGB < srcAlpha, (alpha multiplied in asset)
                // srcRGB * 1 + dstRGB * (1 - srcAlpha)
                case TransparentBlendMode.Premultiply:
                    srcBlendRGB = BlendMode.One;
                    dstBlendRGB = BlendMode.OneMinusSrcAlpha;
                    srcBlendA   = srcBlendRGB;
                    dstBlendA   = dstBlendRGB;
                    break;

                // srcRGB * srcAlpha + dstRGB * 1, (alpha controls amount of addition)
                // preserve spec:
                // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                case TransparentBlendMode.Additive:
                    srcBlendRGB = BlendMode.SrcAlpha;
                    dstBlendRGB = BlendMode.One;
                    srcBlendA   = BlendMode.One;
                    dstBlendA   = dstBlendRGB;
                    break;

                // srcRGB * 0 + dstRGB * srcRGB
                // in shader alpha controls amount of multiplication, lerp(1, srcRGB, srcAlpha)
                // Multiply affects color only, keep existing alpha.
                case TransparentBlendMode.Multiply:
                    srcBlendRGB = BlendMode.DstColor;
                    dstBlendRGB = BlendMode.Zero;
                    srcBlendA   = BlendMode.Zero;
                    dstBlendA   = BlendMode.One;

                    material.EnableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);
                    break;
            }

            return (srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA);
        }

        private static void SetMaterialSrcDstBlendProperties(Material material, BlendMode srcBlend, BlendMode dstBlend)
        {
            material.SetFloat(HumToonPropertyNames.SrcBlend, (float)srcBlend);
            material.SetFloat(HumToonPropertyNames.DstBlend, (float)dstBlend);
            material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlend);
            material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlend);
        }

        private static void SetMaterialSrcDstBlendProperties(Material material, BlendMode srcBlendRGB, BlendMode dstBlendRGB, BlendMode srcBlendAlpha, BlendMode dstBlendAlpha)
        {
            material.SetFloat(HumToonPropertyNames.SrcBlend, (float)srcBlendRGB);
            material.SetFloat(HumToonPropertyNames.DstBlend, (float)dstBlendRGB);
            material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlendAlpha);
            material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlendAlpha);
        }
    }
}
