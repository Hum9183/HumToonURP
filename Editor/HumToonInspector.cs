using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor.Rendering.Universal.ShaderGUI;

namespace HumToon.Editor
{
    public partial class HumToonInspector : ShaderGUI
    {
        // fields
        private MaterialEditor _materialEditor;
        private MaterialPropertySetter _materialPropertySetter;
        private HumToonMaterialPropertyContainer _matPropContainer;
        private LitMaterialPropertyContainer _litMatPropContainer;
        private LitDetailMaterialPropertyContainer _litDetailMatPropContainer;

        private bool _firstTimeApply = true;

        /// <summary>
        /// Foldouts
        /// By default, everything is expanded, except advanced
        /// </summary>
        private readonly MaterialHeaderScopeList _materialHeaderScopeList = new MaterialHeaderScopeList(uint.MaxValue & ~(uint)Expandable.Advanced);

        // methods
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] materialProperties)
        {
            _materialEditor = materialEditor ? materialEditor : throw new ArgumentNullException(nameof(materialEditor));
            Material material = materialEditor.target as Material; // TODO: メンバ化できないか要検証

            if (_firstTimeApply)
            {
                InitializeMaterialPropertyContainers();
                OnOpenGUI();
                _firstTimeApply = false;
            }

            SetMaterialPropertyContainers(materialProperties);
            _materialHeaderScopeList.DrawHeaders(materialEditor, material);
        }

        private void InitializeMaterialPropertyContainers()
        {
            _materialPropertySetter    = new MaterialPropertySetter();
            _matPropContainer          = new HumToonMaterialPropertyContainer(_materialPropertySetter);
            _litMatPropContainer       = new LitMaterialPropertyContainer(_materialPropertySetter);
            _litDetailMatPropContainer = new LitDetailMaterialPropertyContainer(_materialPropertySetter);
        }

        private void SetMaterialPropertyContainers(MaterialProperty[] materialProperties)
        {
            _materialPropertySetter.MatProps = materialProperties;
            _matPropContainer.Set();
            _litMatPropContainer.Set();
            _litDetailMatPropContainer.Set();
        }

        private void OnOpenGUI()
        {
            var filter = (Expandable)materialFilter;

            // NOTE: 現在はGUI描画をpartialで定義したAction<Material>で渡しているが、
            // 第三者の拡張のしやすさを考慮し、Classのインスタンスで渡す形を検討したい。

            // Generate the foldouts
            if (filter.HasFlag(Expandable.SurfaceOptions))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.SurfaceOptions, (uint)Expandable.SurfaceOptions, DrawSurfaceOptions);

            if (filter.HasFlag(Expandable.SurfaceInputs))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.SurfaceInputs, (uint)Expandable.SurfaceInputs, DrawSurfaceInputs);

            if (filter.HasFlag(Expandable.Details))
                FillAdditionalFoldouts(_materialHeaderScopeList);

            if (filter.HasFlag(Expandable.Advanced))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.AdvancedLabel, (uint)Expandable.Advanced, DrawAdvancedOptions);
        }

        // ==============================================================

        private const int queueOffsetRange = 50;

        /// <summary>
        /// Filter for the surface options, surface inputs, details and advanced foldouts.
        /// </summary>
        private uint materialFilter => uint.MaxValue;

        // /// <summary>
        // /// Draws the tile offset GUI.
        // /// </summary>
        // /// <param name="materialEditor">The material editor to use.</param>
        // /// <param name="textureProp">The texture property.</param>
        // protected static void DrawTileOffset(MaterialEditor materialEditor, MaterialProperty textureProp)
        // {
        //     if (textureProp != null)
        //         materialEditor.TextureScaleOffsetProperty(textureProp);
        // }

        // this function is shared between ShaderGraph and hand-written GUIs
        internal static void UpdateMaterialRenderQueueControl(Material material)
        {
            //
            // Render Queue Control handling
            //
            // Check for a raw render queue (the actual serialized setting - material.renderQueue has already been converted)
            // setting of -1, indicating that the material property should be inherited from the shader.
            // If we find this, add a new property "render queue control" set to 0 so we will
            // always know to follow the surface type of the material (this matches the hand-written behavior)
            // If we find another value, add the the property set to 1 so we will know that the
            // user has explicitly selected a render queue and we should not override it.
            //
            bool isShaderGraph = false; // Non-shadergraph materials use automatic behavior // NOTE: ShaderGraphではない
            // int rawRenderQueue = MaterialAccess.ReadMaterialRawRenderQueue(material); // NOTE: internalだから取得できない
            // if (!isShaderGraph || rawRenderQueue == -1)
            if (!isShaderGraph)
            {
                material.SetFloat(HumToonPropertyNames.QueueControl, (float)QueueControl.Auto); // Automatic behavior - surface type override
            }
            else
            {
                material.SetFloat(HumToonPropertyNames.QueueControl, (float)QueueControl.UserOverride); // User has selected explicit render queue
            }
        }

        internal static void SetMaterialSrcDstBlendProperties(Material material, UnityEngine.Rendering.BlendMode srcBlendRGB, UnityEngine.Rendering.BlendMode dstBlendRGB, UnityEngine.Rendering.BlendMode srcBlendAlpha, UnityEngine.Rendering.BlendMode dstBlendAlpha)
        {
            if (material.HasProperty(HumToonPropertyNames.SrcBlend))
                material.SetFloat(HumToonPropertyNames.SrcBlend, (float)srcBlendRGB);

            if (material.HasProperty(HumToonPropertyNames.DstBlend))
                material.SetFloat(HumToonPropertyNames.DstBlend, (float)dstBlendRGB);

            if (material.HasProperty(HumToonPropertyNames.SrcBlendAlpha))
                material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlendAlpha);

            if (material.HasProperty(HumToonPropertyNames.DstBlendAlpha))
                material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlendAlpha);
        }


        /// <summary>
        /// Sets up the blend mode.
        /// </summary>
        /// <param name="material">The material to use.</param>
        public static void SetupMaterialBlendMode(Material material)
        {
            SetupMaterialBlendModeInternal(material, out int renderQueue);

            // apply automatic render queue
            if (renderQueue != material.renderQueue)
                material.renderQueue = renderQueue;
        }

        public static void SetupMaterialBlendModeInternal(Material material, out int automaticRenderQueue)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            bool alphaClip = false;
            if (material.HasProperty(HumToonPropertyNames.AlphaClip))
                alphaClip = material.GetFloat(HumToonPropertyNames.AlphaClip) >= 0.5;
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._ALPHATEST_ON, alphaClip);

            // default is to use the shader render queue
            int renderQueue = material.shader.renderQueue;
            material.SetOverrideTag("RenderType", "");      // clear override tag
            if (material.HasProperty(HumToonPropertyNames.SurfaceType))
            {
                SurfaceType surfaceType = (SurfaceType)material.GetFloat(HumToonPropertyNames.SurfaceType);
                bool zwrite = false;
                CoreUtils.SetKeyword(material, ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT, surfaceType == SurfaceType.Transparent);
                bool alphaToMask = false;
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
                    BlendMode blendMode = (BlendMode)material.GetFloat(HumToonPropertyNames.BlendMode);

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
                    bool preserveSpecular = (material.HasProperty(HumToonPropertyNames.BlendModePreserveSpecular) &&
                                             material.GetFloat(HumToonPropertyNames.BlendModePreserveSpecular) > 0) &&
                                            blendMode != BlendMode.Multiply && blendMode != BlendMode.Premultiply;
                    if (preserveSpecular)
                    {
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                        material.EnableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
                    }

                    // When doing off-screen transparency accumulation, we change blend factors as described here: https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
                    bool offScreenAccumulateAlpha = false;
                    if (offScreenAccumulateAlpha)
                        srcBlendA = UnityEngine.Rendering.BlendMode.Zero;

                    SetMaterialSrcDstBlendProperties(material, srcBlendRGB, dstBlendRGB, // RGB
                        srcBlendA, dstBlendA); // Alpha

                    // General Transparent Material Settings
                    material.SetOverrideTag("RenderType", "Transparent");
                    zwrite = false;
                    material.EnableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
                    renderQueue = (int)RenderQueue.Transparent;
                }

                if (material.HasProperty(HumToonPropertyNames.AlphaToMask))
                {
                    material.SetFloat(HumToonPropertyNames.AlphaToMask, alphaToMask ? 1.0f : 0.0f);
                }

                // check for override enum
                if (material.HasProperty(HumToonPropertyNames.ZWriteControl))
                {
                    var zwriteControl = (ZWriteControl)material.GetFloat(HumToonPropertyNames.ZWriteControl);
                    if (zwriteControl == ZWriteControl.ForceEnabled)
                        zwrite = true;
                    else if (zwriteControl == ZWriteControl.ForceDisabled)
                        zwrite = false;
                }
                SetMaterialZWriteProperty(material, zwrite);
                material.SetShaderPassEnabled("DepthOnly", zwrite);
            }
            else
            {
                // no surface type property -- must be hard-coded by the shadergraph,
                // so ensure the pass is enabled at the material level
                material.SetShaderPassEnabled("DepthOnly", true);
            }

            // must always apply queue offset, even if not set to material control
            if (material.HasProperty(HumToonPropertyNames.QueueOffset))
                renderQueue += (int)material.GetFloat(HumToonPropertyNames.QueueOffset);

            automaticRenderQueue = renderQueue;
        }

        internal static void SetMaterialSrcDstBlendProperties(Material material, UnityEngine.Rendering.BlendMode srcBlend, UnityEngine.Rendering.BlendMode dstBlend)
        {
            if (material.HasProperty(HumToonPropertyNames.SrcBlend))
                material.SetFloat(HumToonPropertyNames.SrcBlend, (float)srcBlend);

            if (material.HasProperty(HumToonPropertyNames.DstBlend))
                material.SetFloat(HumToonPropertyNames.DstBlend, (float)dstBlend);

            if (material.HasProperty(HumToonPropertyNames.SrcBlendAlpha))
                material.SetFloat(HumToonPropertyNames.SrcBlendAlpha, (float)srcBlend);

            if (material.HasProperty(HumToonPropertyNames.DstBlendAlpha))
                material.SetFloat(HumToonPropertyNames.DstBlendAlpha, (float)dstBlend);
        }


        internal static bool GetAutomaticQueueControlSetting(Material material)
        {
            // If a Shader Graph material doesn't yet have the queue control property,
            // we should not engage automatic behavior until the shader gets reimported.
            // bool automaticQueueControl = !material.IsShaderGraph(); // NOTE: ShaderGraphではない
            bool automaticQueueControl = true;
            if (material.HasProperty(HumToonPropertyNames.QueueControl))
            {
                var queueControl = material.GetFloat(HumToonPropertyNames.QueueControl);
                if (queueControl < 0.0f)
                {
                    // The property was added with a negative value, indicating it needs to be validated for this material
                    UpdateMaterialRenderQueueControl(material);
                }
                automaticQueueControl = (material.GetFloat(HumToonPropertyNames.QueueControl) == (float)QueueControl.Auto);
            }
            return automaticQueueControl;
        }

        internal static bool IsOpaque(Material material)
        {
            bool opaque = true;
            if (material.HasProperty(HumToonPropertyNames.SurfaceType))
                opaque = ((SurfaceType)material.GetFloat(HumToonPropertyNames.SurfaceType) == SurfaceType.Opaque);
            return opaque;
        }

        /// <summary>
        /// Helper function to draw a popup.
        /// </summary>
        /// <param name="label">The label to use.</param>
        /// <param name="property">The property to display.</param>
        /// <param name="options">The options available.</param>
        public void DoPopup(GUIContent label, MaterialProperty property, string[] options)
        {
            if (property != null)
                _materialEditor.PopupShaderProperty(property, label, options);
        }

        // /// <summary>
        // /// Draws the surface inputs GUI.
        // /// </summary>
        // /// <param name="material">The material to use.</param>
        // private void DrawSurfaceInputs(Material material)
        // {
        //     DrawBaseProperties(material);
        //
        //     // lit専用
        //     LitGUI.Inputs(_litMatPropContainer, _materialEditor, material);
        //     DrawEmissionProperties(material, true);
        //     DrawTileOffset(_matPropContainer.BaseMap);
        // }

                /// <summary>
        /// Helper function to show texture and color properties.
        /// </summary>
        /// <param name="materialEditor">The material editor to use.</param>
        /// <param name="label">The label to use.</param>
        /// <param name="textureProp">The texture property.</param>
        /// <param name="colorProp">The color property.</param>
        /// <param name="hdr">Marks whether this is a HDR texture or not.</param>
        /// <returns></returns>
        public static Rect TextureColorProps(MaterialEditor materialEditor, GUIContent label, MaterialProperty textureProp, MaterialProperty colorProp, bool hdr = false)
        {
            MaterialEditor.BeginProperty(textureProp);
            if (colorProp != null)
                MaterialEditor.BeginProperty(colorProp);

            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.showMixedValue = textureProp.hasMixedValue;
            materialEditor.TexturePropertyMiniThumbnail(rect, textureProp, label.text, label.tooltip);
            EditorGUI.showMixedValue = false;

            if (colorProp != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = colorProp.hasMixedValue;
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                Rect rectAfterLabel = new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y,
                    EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
                var col = EditorGUI.ColorField(rectAfterLabel, GUIContent.none, colorProp.colorValue, true,
                    false, hdr);
                EditorGUI.indentLevel = indentLevel;
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(colorProp.displayName);
                    colorProp.colorValue = col;
                }
                EditorGUI.showMixedValue = false;
            }

            if (colorProp != null)
                MaterialEditor.EndProperty();
            MaterialEditor.EndProperty();

            return rect;
        }

        /// <summary>
        /// Draws the GUI for the normal area.
        /// </summary>
        /// <param name="materialEditor">The material editor to use.</param>
        /// <param name="bumpMap">The normal map property.</param>
        /// <param name="bumpMapScale">The normal map scale property.</param>
        public static void DrawNormalArea(MaterialEditor materialEditor, MaterialProperty bumpMap, MaterialProperty bumpMapScale = null)
        {
            if (bumpMapScale != null)
            {
                materialEditor.TexturePropertySingleLine(HumToonStyles.NormalMap, bumpMap,
                    bumpMap.textureValue != null ? bumpMapScale : null);
                if (bumpMapScale.floatValue != 1 &&
                    UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(
                        EditorUserBuildSettings.activeBuildTarget))
                    if (materialEditor.HelpBoxWithButton(HumToonStyles.BumpScaleNotSupported, HumToonStyles.FixNormalNow))
                        bumpMapScale.floatValue = 1;
            }
            else
            {
                materialEditor.TexturePropertySingleLine(HumToonStyles.NormalMap, bumpMap);
            }
        }


        /// <summary>
        /// Draws the base properties GUI.
        /// </summary>
        /// <param name="material">The material to use.</param>
        private void DrawBaseProperties(Material material)
        {
            if (_matPropContainer.BaseMap != null && _matPropContainer.BaseColor != null) // Draw the baseMap, most shader will have at least a baseMap
            {
                _materialEditor.TexturePropertySingleLine(HumToonStyles.BaseMap, _matPropContainer.BaseMap, _matPropContainer.BaseColor);
            }
        }

        private void DrawEmissionTextureProperty()
        {
            if ((_matPropContainer.EmissionMap == null) || (_matPropContainer.EmissionColor == null))
                return;

            using (new EditorGUI.IndentLevelScope(2))
            {
                _materialEditor.TexturePropertyWithHDRColor(HumToonStyles.EmissionMap, _matPropContainer.EmissionMap, _matPropContainer.EmissionColor, false);
            }
        }

        /// <summary>
        /// Draws the advanced options GUI.
        /// </summary>
        /// <param name="material">The material to use.</param>
        public virtual void DrawAdvancedOptions(Material material)
        {
            // lit専用
            if (_litMatPropContainer.EnvironmentReflections != null && _litMatPropContainer.SpecularHighlights != null)
            {
                _materialEditor.ShaderProperty(_litMatPropContainer.SpecularHighlights, LitStyles.Highlights);
                _materialEditor.ShaderProperty(_litMatPropContainer.EnvironmentReflections, LitStyles.Reflections);
            }

            // Only draw the sorting priority field if queue control is set to "auto"
            bool autoQueueControl = GetAutomaticQueueControlSetting(material);
            if (autoQueueControl)
                DrawQueueOffsetField();
            _materialEditor.EnableInstancingField();
        }

        // this is the function used by Lit.shader, Unlit.shader GUIs
        /// <summary>
        /// Sets up the keywords for the material and shader.
        /// </summary>
        /// <param name="material">The material to use.</param>
        /// <param name="shadingModelFunc">Function to set shading models.</param>
        /// <param name="shaderFunc">Function to set some extra shader parameters.</param>
        public static void SetMaterialKeywords(Material material, Action<Material> shadingModelFunc = null, Action<Material> shaderFunc = null)
        {
            UpdateMaterialSurfaceOptions(material, automaticRenderQueue: true);

            // Setup double sided GI based on Cull state
            if (material.HasProperty(HumToonPropertyNames.CullMode))
                material.doubleSidedGI = (RenderFace)material.GetFloat(HumToonPropertyNames.CullMode) != RenderFace.Front;

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
            if (material.HasProperty(HumToonPropertyNames.EmissionColor))
                MaterialEditor.FixupEmissiveFlag(material);

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

        // this function is shared with ShaderGraph Lit/Unlit GUIs and also the hand-written GUIs
        private static void UpdateMaterialSurfaceOptions(Material material, bool automaticRenderQueue)
        {
            // Setup blending - consistent across all Universal RP shaders
            SetupMaterialBlendModeInternal(material, out int renderQueue);

            // apply automatic render queue
            if (automaticRenderQueue && (renderQueue != material.renderQueue))
                material.renderQueue = renderQueue;

            bool isShaderGraph = false;

            // Cast Shadows
            bool castShadows = true;
            if (material.HasProperty(HumToonPropertyNames.CastShadows))
            {
                castShadows = (material.GetFloat(HumToonPropertyNames.CastShadows) != 0.0f);
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
                    castShadows = IsOpaque(material);
                }
            }
            material.SetShaderPassEnabled("ShadowCaster", castShadows);

            // Receive Shadows
            if (material.HasProperty(HumToonPropertyNames.ReceiveShadows))
                CoreUtils.SetKeyword(material, ShaderKeywordStrings._RECEIVE_SHADOWS_OFF, material.GetFloat(HumToonPropertyNames.ReceiveShadows) == 0.0f);
        }

        /// <summary>
        /// Draws the queue offset field.
        /// </summary>
        protected void DrawQueueOffsetField()
        {
            if (_matPropContainer.QueueOffset != null)
                _materialEditor.IntSliderShaderProperty(_matPropContainer.QueueOffset, -queueOffsetRange, queueOffsetRange, HumToonStyles.QueueSlider);
        }

        /// <summary>
        /// Called when a material has been changed.
        /// This function has been deprecated and has been renamed to ValidateMaterial.
        /// </summary>
        /// <param name="material">The material that has been changed.</param>
        [Obsolete("MaterialChanged has been renamed ValidateMaterial", false)]
        public virtual void MaterialChanged(Material material)
        {
            ValidateMaterial(material);
        }

        public void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            materialScopesList.RegisterHeaderScope(LitDetailGUI.Styles.detailInputs, Expandable.Details, _ => LitDetailGUI.DoDetailArea(_litDetailMatPropContainer, _materialEditor));
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
        }

        internal static void SetMaterialZWriteProperty(Material material, bool zwriteEnabled)
        {
            if (material.HasProperty(HumToonPropertyNames.ZWrite))
                material.SetFloat(HumToonPropertyNames.ZWrite, zwriteEnabled ? 1.0f : 0.0f);
        }

        // Copied from shaderGUI as it is a protected function in an abstract class, unavailable to others
        /// <summary>
        /// Searches and tries to find a property in an array of properties.
        /// </summary>
        /// <param name="propertyName">The property to find.</param>
        /// <param name="properties">Array of properties to search in.</param>
        /// <param name="propertyIsMandatory">Should throw exception if property is not found</param>
        /// <returns>A MaterialProperty instance for the property.</returns>
        /// <exception cref="ArgumentException"></exception>
        public new static MaterialProperty FindProperty(string propertyName, MaterialProperty[] properties, bool propertyIsMandatory)
        {
            for (int index = 0; index < properties.Length; ++index)
            {
                if (properties[index] != null && properties[index].name == propertyName)
                    return properties[index];
            }
            if (propertyIsMandatory)
                throw new ArgumentException("Could not find MaterialProperty: '" + propertyName + "', Num properties: " + (object)properties.Length);
            return null;
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;
            base.AssignNewShaderToMaterial(material, oldShader, newShader);
            // Setup keywords based on the new shader
            UpdateMaterial(material, MaterialUpdateType.ChangedAssignedShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Blend", (float)blendMode);

            material.SetFloat("_Surface", (float)surfaceType);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }

            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
        }


        // this is used to update a material's keywords, applying any shader-associated logic to update dependent properties and keywords
        // this is also invoked when a material is created, modified, or the material's shader is modified or reassigned
        internal static void UpdateMaterial(Material material, MaterialUpdateType updateType)
        {
            // if unknown, look it up from the material's shader
            // NOTE: this will only work for asset-based shaders..
            // if (shaderID == ShaderID.Unknown)
            //     shaderID = GetShaderID(material.shader);

            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords);
        }
    }
}
