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
    }
}
