using System;
using System.Collections.Generic;
using Hum.HumToon.Editor.HeaderScopes;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor
{
    public class HumToonInspector : ShaderGUI
    {
        // TODO:
        // ・日本語対応

        private MaterialEditor _materialEditor;
        private readonly PropertySetter _propSetter = new PropertySetter();
        private IEnumerable<IHeaderScopeDrawer> _drawers;
        private bool _firstTimeApply = true;

        /// <summary>
        /// インスペクタ上をマウスが動いているときに呼ばれる
        /// </summary>
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] materialProperties)
        {
            _materialEditor = materialEditor ? materialEditor : throw new ArgumentNullException(nameof(materialEditor));

            if (_firstTimeApply)
            {
                InitDrawers();
                _firstTimeApply = false;
            }

            SetMaterialProperties(materialProperties);
            Draw();
        }

        private void InitDrawers()
        {
            var factory = new HeaderScopeFactory();
            _drawers = factory.CreateDrawers(_propSetter);
        }

        /// <summary>
        /// マテリアルプロパティは毎描画ごとに取得する
        /// </summary>
        private void SetMaterialProperties(MaterialProperty[] materialProperties)
        {
            _propSetter.MatProps = materialProperties; // NOTE: 各DrawersたちはPropertySetterへの参照を持っている

            foreach (var drawer in _drawers)
            {
                drawer.SetProperties();
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        private void Draw()
        {
            EditorGUIUtility.labelWidth = 0f;

            foreach (var drawer in _drawers)
            {
                drawer.Draw(_materialEditor);
            }
        }

        /// <summary>
        /// Called when a material has been changed.
        /// </summary>
        public override void ValidateMaterial(Material material)
        {
            var factory = new HeaderScopeFactory();
            foreach (var validator in factory.CreateValidators())
            {
                validator.Validate(material);
            }
        }

        /// <summary>
        /// NOTE: 自身がnewShaderのときに呼ばれる
        /// </summary>
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material is null)
                throw new ArgumentNullException(nameof(material));

            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;

            // Assign new shader
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader is null || oldShader.name.Contains("Legacy Shaders/") is false)
            {
                // Setup keywords based on the new shader
                ValidateMaterial(material);
            }

            // NOTE: Legacy Shadersはサポートしない
        }
    }
}
