using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public static class BlendSetter
    {
        public static void Set(Material material, bool isOpaque, TransparentBlendMode transparentBlendMode, bool preserveSpecular)
        {
            if (isOpaque)
            {
                SetSrcDst(material, BlendMode.One, BlendMode.Zero);
            }
            else
            {
                var (srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA) = SwitchBlendMode(transparentBlendMode);
                if (preserveSpecular)
                    srcBlendRGB = BlendMode.One;
                SetSrcDst(material, srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA);
            }
        }

        private static (BlendMode srcBlendRGB, BlendMode dstBlendRGB, BlendMode srcBlendA, BlendMode dstBlendA)
            SwitchBlendMode(TransparentBlendMode transparentBlendMode)
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
                    break;
            }

            return (srcBlendRGB, dstBlendRGB, srcBlendA, dstBlendA);
        }

        private static void SetSrcDst(Material material, BlendMode srcBlend, BlendMode dstBlend)
        {
            material.SetFloat(HumToonPropertyNames.SrcBlend,      (float)srcBlend);
            material.SetFloat(HumToonPropertyNames.DstBlend,      (float)dstBlend);
            material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlend);
            material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlend);
        }

        private static void SetSrcDst(Material material, BlendMode srcBlendRGB, BlendMode dstBlendRGB, BlendMode srcBlendAlpha, BlendMode dstBlendAlpha)
        {
            material.SetFloat(HumToonPropertyNames.SrcBlend,      (float)srcBlendRGB);
            material.SetFloat(HumToonPropertyNames.DstBlend,      (float)dstBlendRGB);
            material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlendAlpha);
            material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlendAlpha);
        }
    }
}
