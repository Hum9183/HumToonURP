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
// using static Unity.Rendering.Universal.ShaderUtils;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace HumToon.Editor
{
    internal partial class HumToonGUI : ShaderGUI
    {
        private uint                             MaterialFilter => uint.MaxValue;
        private bool                             _firstTimeApply = true;
        private MaterialEditor                   _materialEditor;
        private readonly MaterialProperties      _matProps = new MaterialProperties();
        private readonly MaterialHeaderScopeList _materialHeaderScopeList = new MaterialHeaderScopeList(uint.MaxValue & ~(uint)Expandable.Advanced);
        private const int                        QueueOffsetRange = 50;

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] matProps)
        {
            _materialEditor = materialEditorIn ? materialEditorIn : throw new ArgumentNullException(nameof(materialEditorIn));
            Material material = _materialEditor.target as Material;

            _matProps.Set(matProps);

            if (_firstTimeApply)
            {
                OnOpenGUI();
                _firstTimeApply = false;
            }

            _materialHeaderScopeList.DrawHeaders(_materialEditor, material);

            FinalSetting(material);
        }
        
        private void OnOpenGUI()
        {
            _materialHeaderScopeList.RegisterHeaderScope(Styles.SurfaceOptions, Expandable.SurfaceOptions, DrawSurfaceOptions);
            _materialHeaderScopeList.RegisterHeaderScope(Styles.SurfaceInputs,  Expandable.SurfaceInputs,  DrawSurfaceInputs);
            _materialHeaderScopeList.RegisterHeaderScope(Styles.AdvancedLabel,  Expandable.Advanced,       DrawAdvancedOptions);
        }
        
        private void FinalSetting(Material material)
        {
            SetupMaterialBlendModeInternal(material, out int renderQueue);
            UpdateMaterialSurfaceOptions(material, automaticRenderQueue: true, renderQueue);
            SetMaterialKeywords(material);
        }

        private void SetMaterialKeywords(Material material, Action<Material> shadingModelFunc = null, Action<Material> shaderFunc = null)
        {

            // Setup double sided GI based on Cull state
            // if (material.HasProperty(MaterialPropertyNames.CullMode))
            material.doubleSidedGI = (RenderFace)_matProps.CullMode.floatValue != RenderFace.Front;

            // Temporary fix for lightmapping. TODO: to be replaced with attribute tag.
            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", material.GetTexture("_BaseMap"));
                material.SetTextureScale("_MainTex", material.GetTextureScale("_BaseMap"));
                material.SetTextureOffset("_MainTex", material.GetTextureOffset("_BaseMap"));
            }
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", material.GetColor("_BaseColor"));

            // Emission
            // if (material.HasProperty(MaterialPropertyNames.EmissionColor))
            //     MaterialEditor.FixupEmissiveFlag(material);

            bool shouldEmissionBeEnabled =
                (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;

            // Not sure what this is used for, I don't see this property declared by any Unity shader in our repo...
            // I'm guessing it is some kind of legacy material upgrade support thing?  Or maybe just dead code now...
            if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
                shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;

            CoreUtils.SetKeyword(material, ShaderKeywordStrings._EMISSION, shouldEmissionBeEnabled);

            // Normal Map
            if (material.HasProperty("_BumpMap"))
                CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, material.GetTexture("_BumpMap"));

            // Shader specific keyword functions
            shadingModelFunc?.Invoke(material);
            shaderFunc?.Invoke(material);
        }

        private void SetupMaterialBlendModeInternal(Material material, out int automaticRenderQueue)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            material.SetOverrideTag("RenderType", "");      // clear override tag
            
            // alpha clip
            bool alphaClip = false;
            alphaClip = (HumToggle)_matProps.AlphaClip.floatValue is HumToggle.On;
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);

            int renderQueue = material.shader.renderQueue;
            bool zwrite = false;
            bool alphaToMask = false;

            // transparent
            SurfaceType surfaceType = (SurfaceType)_matProps.SurfaceType.floatValue;
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT, surfaceType is SurfaceType.Transparent);
            
            if (surfaceType == SurfaceType.Opaque)
            {
                if (alphaClip)
                {
                    renderQueue = (int)RenderQueue.AlphaTest;
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    alphaToMask = true;
                }
                else
                {
                    renderQueue = (int)RenderQueue.Geometry;
                    material.SetOverrideTag("RenderType", "Opaque");
                }

                SetMaterialSrcDstBlendProperties(material, UnityEngine.Rendering.BlendMode.One, UnityEngine.Rendering.BlendMode.Zero);
                zwrite = true;
                material.DisableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
                material.DisableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
                material.DisableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);
            }
            else // SurfaceType Transparent
            {
                BlendMode blendMode = (BlendMode)_matProps.BlendMode.floatValue;

                // Clear blend keyword state.
                material.DisableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
                material.DisableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);

                var srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                var dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                var srcBlendA = UnityEngine.Rendering.BlendMode.One;
                var dstBlendA = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;

                // Specific Transparent Mode Settings
                switch (blendMode)
                {
                    // srcRGB * srcAlpha + dstRGB * (1 - srcAlpha)
                    // preserve spec:
                    // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                    case BlendMode.Alpha:
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.SrcAlpha;
                        dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                        srcBlendA = UnityEngine.Rendering.BlendMode.One;
                        dstBlendA = dstBlendRGB;
                        break;

                    // srcRGB < srcAlpha, (alpha multiplied in asset)
                    // srcRGB * 1 + dstRGB * (1 - srcAlpha)
                    case BlendMode.Premultiply:
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                        dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                        srcBlendA = srcBlendRGB;
                        dstBlendA = dstBlendRGB;
                        break;

                    // srcRGB * srcAlpha + dstRGB * 1, (alpha controls amount of addition)
                    // preserve spec:
                    // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                    case BlendMode.Additive:
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.SrcAlpha;
                        dstBlendRGB = UnityEngine.Rendering.BlendMode.One;
                        srcBlendA = UnityEngine.Rendering.BlendMode.One;
                        dstBlendA = dstBlendRGB;
                        break;

                    // srcRGB * 0 + dstRGB * srcRGB
                    // in shader alpha controls amount of multiplication, lerp(1, srcRGB, srcAlpha)
                    // Multiply affects color only, keep existing alpha.
                    case BlendMode.Multiply:
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.DstColor;
                        dstBlendRGB = UnityEngine.Rendering.BlendMode.Zero;
                        srcBlendA = UnityEngine.Rendering.BlendMode.Zero;
                        dstBlendA = UnityEngine.Rendering.BlendMode.One;

                        material.EnableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);
                        break;
                }

                // Lift alpha multiply from ROP to shader by setting pre-multiplied _SrcBlend mode.
                // The intent is to do different blending for diffuse and specular in shader.
                // ref: http://advances.realtimerendering.com/other/2016/naughty_dog/NaughtyDog_TechArt_Final.pdf

                SetMaterialSrcDstBlendProperties(material, srcBlendRGB, dstBlendRGB, // RGB
                    srcBlendA, dstBlendA); // Alpha

                // General Transparent Material Settings
                material.SetOverrideTag("RenderType", "Transparent");
                zwrite = false;
                material.EnableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
                renderQueue = (int)RenderQueue.Transparent;
            }

            if (material.HasProperty(MaterialPropertyNames.AlphaToMask))
            {
                material.SetFloat(MaterialPropertyNames.AlphaToMask, alphaToMask ? 1.0f : 0.0f);
            }

            // check for override enum
            if (material.HasProperty(MaterialPropertyNames.ZWriteControl))
            {
                var zwriteControl = (ZWriteControl)material.GetFloat(MaterialPropertyNames.ZWriteControl);
                if (zwriteControl == ZWriteControl.ForceEnabled)
                    zwrite = true;
                else if (zwriteControl == ZWriteControl.ForceDisabled)
                    zwrite = false;
            }
            SetMaterialZWriteProperty(material, zwrite);
            material.SetShaderPassEnabled("DepthOnly", zwrite);


            // must always apply queue offset, even if not set to material control
            if (material.HasProperty(MaterialPropertyNames.QueueOffset))
                renderQueue += (int)material.GetFloat(MaterialPropertyNames.QueueOffset);

            automaticRenderQueue = renderQueue;
        }
        
        private void SetMaterialSrcDstBlendProperties(Material material, UnityEngine.Rendering.BlendMode srcBlend, UnityEngine.Rendering.BlendMode dstBlend)
        {
            material.SetFloat(MaterialPropertyNames.SrcBlend, (float)srcBlend);
            material.SetFloat(MaterialPropertyNames.DstBlend, (float)dstBlend);
            material.SetFloat(MaterialPropertyNames.SrcBlendAlpha, (float)srcBlend);
            material.SetFloat(MaterialPropertyNames.DstBlendAlpha, (float)dstBlend);
        }
        
        private void UpdateMaterialSurfaceOptions(Material material, bool automaticRenderQueue, int renderQueue)
        {
            // Setup blending - consistent across all Universal RP shaders

            // apply automatic render queue
            if (automaticRenderQueue && (renderQueue != material.renderQueue))
                material.renderQueue = renderQueue;

            bool isShaderGraph = material.IsShaderGraph();

            // Cast Shadows
            bool castShadows = true;
            if (material.HasProperty(MaterialPropertyNames.CastShadows))
            {
                castShadows = (material.GetFloat(MaterialPropertyNames.CastShadows) != 0.0f);
            }
            else
            {
                if (isShaderGraph)
                {
                    // Lit.shadergraph or Unlit.shadergraph, but no material control defined
                    // enable the pass in the material, so shader can decide...
                    castShadows = true;
                }
                else
                {
                    // Lit.shader or Unlit.shader -- set based on transparency
                    // castShadows = Rendering.Universal.ShaderGUI.LitGUI.IsOpaque(material);
                    // TODO: 多分この処理で代用できるはず
                    castShadows = IsOpaque(material);
                }
            }
            material.SetShaderPassEnabled("ShadowCaster", castShadows);

            // Receive Shadows
            if (material.HasProperty(MaterialPropertyNames.ReceiveShadows))
                CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, material.GetFloat(MaterialPropertyNames.ReceiveShadows) == 0.0f);
        }
        internal static bool IsOpaque(Material material)
        {
            bool opaque = true;
            if (material.HasProperty(MaterialPropertyNames.SurfaceType))
                opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat(MaterialPropertyNames.SurfaceType) == BaseShaderGUI.SurfaceType.Opaque);
            return opaque;
        }
        
        private void SetMaterialSrcDstBlendProperties(Material material, UnityEngine.Rendering.BlendMode srcBlendRGB, UnityEngine.Rendering.BlendMode dstBlendRGB, UnityEngine.Rendering.BlendMode srcBlendAlpha, UnityEngine.Rendering.BlendMode dstBlendAlpha)
        {
            if (material.HasProperty(MaterialPropertyNames.SrcBlend))
                material.SetFloat(MaterialPropertyNames.SrcBlend, (float)srcBlendRGB);

            if (material.HasProperty(MaterialPropertyNames.DstBlend))
                material.SetFloat(MaterialPropertyNames.DstBlend, (float)dstBlendRGB);

            if (material.HasProperty(MaterialPropertyNames.SrcBlendAlpha))
                material.SetFloat(MaterialPropertyNames.SrcBlendAlpha, (float)srcBlendAlpha);

            if (material.HasProperty(MaterialPropertyNames.DstBlendAlpha))
                material.SetFloat(MaterialPropertyNames.DstBlendAlpha, (float)dstBlendAlpha);
        }
        
        private void SetMaterialZWriteProperty(Material material, bool zwriteEnabled)
        {
            if (material.HasProperty(MaterialPropertyNames.ZWrite))
                material.SetFloat(MaterialPropertyNames.ZWrite, zwriteEnabled ? 1.0f : 0.0f);
        }
        
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            // Setup keywords based on the new shader
            // UpdateMaterial(material);
        }
        
        // private void UpdateMaterial(Material material)
        // {
        //     SetMaterialKeywords(material);
        // }
    }
}
