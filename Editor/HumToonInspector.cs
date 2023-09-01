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

        /// <summary>
        /// Called when a material has been changed.
        /// </summary>
        public override void ValidateMaterial(Material material)
        {
            int renderQueue = MaterialBlendModeSetter.Set(material);
            Utils.UpdateMaterialRenderQueue(material, renderQueue);

            MaterialKeywordsSetter.Set(material, litDetail: true);
        }

        /// <summary>
        /// NOTE: 自身がnewShaderのときに呼ばれる
        /// </summary>
        /// <param name="material"></param>
        /// <param name="oldShader"></param>
        /// <param name="newShader"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material is null)
                throw new ArgumentNullException(nameof(material));

            AssignNewShaderToMaterialUtils.ConvertEmission(material);

            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;

            // Assign new shader
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            // Setup keywords based on the new shader
            MaterialKeywordsSetter.Set(material);

            if (oldShader is null || oldShader.name.Contains("Legacy Shaders/") is false)
            {
                int renderQueue = MaterialBlendModeSetter.Set(material);
                Utils.UpdateMaterialRenderQueue(material, renderQueue);
            }
            else
            {
                AssignNewShaderToMaterialUtils.ModeLegacyShaders(material, oldShader);
            }
        }
    }
}
