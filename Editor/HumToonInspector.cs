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
        // Fields
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

        /// <summary>
        /// Filter for the surface options, surface inputs, details and advanced foldouts.
        /// </summary>
        private static uint MaterialFilter => uint.MaxValue;

        // Constants
        private const int QueueOffsetRange = 50;

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
            var filter = (Expandable)MaterialFilter;

            // NOTE: 現在はGUI描画をpartialで定義したAction<Material>で渡しているが、
            // 第三者の拡張のしやすさを考慮し、Classのインスタンスで渡す形を検討したい。

            // Generate the foldouts
            if (filter.HasFlag(Expandable.SurfaceOptions))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.SurfaceOptions, (uint)Expandable.SurfaceOptions, DrawSurfaceOptions);

            if (filter.HasFlag(Expandable.SurfaceInputs))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.SurfaceInputs, (uint)Expandable.SurfaceInputs, DrawSurfaceInputs);

            if (filter.HasFlag(Expandable.Details))
                _materialHeaderScopeList.RegisterHeaderScope(LitDetailStyles.detailInputs, (uint)Expandable.Details, DrawDetailInputs);

            if (filter.HasFlag(Expandable.Advanced))
                _materialHeaderScopeList.RegisterHeaderScope(HumToonStyles.AdvancedLabel, (uint)Expandable.Advanced, DrawAdvancedOptions);
        }

        // ==============================================================

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material is null)
                throw new ArgumentNullException(nameof(material));

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
            TransparentBlendMode transparentBlendMode = TransparentBlendMode.Alpha;
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
                transparentBlendMode = TransparentBlendMode.Alpha;
            }
            material.SetFloat("_Blend", (float)transparentBlendMode);

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



        /// <summary>
        /// this is used to update a material's keywords, applying any shader-associated logic to update dependent properties and keywords
        /// this is also invoked when a material is created, modified, or the material's shader is modified or reassigned
        /// </summary>
        private void UpdateMaterial(Material material, MaterialUpdateType updateType)
        {
            Debug.Log("UpdateMaterial");
            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords);
        }




        /// <summary>
        /// Sets up the blend mode.
        /// </summary>
        /// <param name="material">The material to use.</param>
        private static void SetupMaterialBlendMode(Material material)
        {
            MaterialBlendModeSetter.Setup(material, out int renderQueue);

            // apply automatic render queue
            if (renderQueue != material.renderQueue)
                material.renderQueue = renderQueue;
        }


        public static bool IsOpaque(Material material)
        {
            return (SurfaceType)material.GetFloat(HumToonPropertyNames.SurfaceType) is SurfaceType.Opaque;
        }





    }
}
