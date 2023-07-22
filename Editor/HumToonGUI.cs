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
using static Unity.Rendering.Universal.ShaderUtils;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace HumToon.Editor
{
    public class HumToonGUI : ShaderGUI
    {
        protected virtual uint _materialFilter => uint.MaxValue;
        public bool FirstTimeApply = true;
        private MaterialEditor _materialEditor;
        private readonly MaterialHeaderScopeList _materialScopeList = new MaterialHeaderScopeList(uint.MaxValue & ~(uint)Expandable.Advanced);
        private MaterialProperty _surfaceTypeProp;
        private MaterialProperty _blendModeProp;

        private MaterialProperty _preserveSpecProp;

        private MaterialProperty _cullingProp;

        private MaterialProperty _ztestProp;

        private MaterialProperty _zwriteProp;

        private MaterialProperty _alphaClipProp;

        private MaterialProperty _alphaCutoffProp;

        private MaterialProperty _castShadowsProp;

        private MaterialProperty _receiveShadowsProp;

        private MaterialProperty _baseMapProp;

        private MaterialProperty _baseColorProp;

        private MaterialProperty _emissionMapProp;
        
        private MaterialProperty _emissionColorProp;

        private MaterialProperty _queueOffsetProp;

        private MaterialProperty _queueControlProp;
        
        private const int QueueOffsetRange = 50;
        

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
        {
            if (materialEditorIn == null)
                throw new ArgumentNullException("materialEditorIn");

            _materialEditor = materialEditorIn;
            Material material = _materialEditor.target as Material;

            FindProperties(properties);   // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly

            // Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
            // material to a universal shader.
            if (FirstTimeApply)
            {
                OnOpenGUI(material, materialEditorIn);
                FirstTimeApply = false;
            }

            ShaderPropertiesGUI(material);
        }
        
        private void FindProperties(MaterialProperty[] properties)
        {
            var material = _materialEditor.target as Material;
            if (material == null)
                return;
            
            _surfaceTypeProp = FindProperty(Property.SurfaceType, properties, false);
            _blendModeProp = FindProperty(Property.BlendMode, properties, false);
            _preserveSpecProp = FindProperty(Property.BlendModePreserveSpecular, properties, false);  // Separate blend for diffuse and specular.
            _cullingProp = FindProperty(Property.CullMode, properties, false);
            _zwriteProp = FindProperty(Property.ZWriteControl, properties, false);
            _ztestProp = FindProperty(Property.ZTest, properties, false);
            _alphaClipProp = FindProperty(Property.AlphaClip, properties, false);

            // ShaderGraph Lit and Unlit Subtargets only
            _castShadowsProp = FindProperty(Property.CastShadows, properties, false);
            _queueControlProp = FindProperty(Property.QueueControl, properties, false);

            // ShaderGraph Lit, and Lit.shader
            _receiveShadowsProp = FindProperty(Property.ReceiveShadows, properties, false);

            // The following are not mandatory for shadergraphs (it's up to the user to add them to their graph)
            _alphaCutoffProp = FindProperty("_Cutoff", properties, false);
            _baseMapProp = FindProperty("_BaseMap", properties, false);
            _baseColorProp = FindProperty("_BaseColor", properties, false);
            _emissionMapProp = FindProperty(Property.EmissionMap, properties, false);
            _emissionColorProp = FindProperty(Property.EmissionColor, properties, false);
            _queueOffsetProp = FindProperty(Property.QueueOffset, properties, false);
        }

        private void DrawSurfaceInputs(Material material)
        {
            DrawBaseProperties(material);
        }
        
        private void DrawBaseProperties(Material material)
        {
            if (_baseMapProp != null && _baseColorProp != null) // Draw the baseMap, most shader will have at least a baseMap
            {
                _materialEditor.TexturePropertySingleLine(Styles.baseMap, _baseMapProp, _baseColorProp);
            }
        }
        
        private void DrawAdvancedOptions(Material material)
        {
            // Only draw the sorting priority field if queue control is set to "auto"
            bool autoQueueControl = GetAutomaticQueueControlSetting(material);
            if (autoQueueControl)
                DrawQueueOffsetField();
            _materialEditor.EnableInstancingField();
        }
        private void DrawQueueOffsetField()
        {
            if (_queueOffsetProp != null)
                _materialEditor.IntSliderShaderProperty(_queueOffsetProp, -QueueOffsetRange, QueueOffsetRange, Styles.queueSlider);
        }
        internal static bool GetAutomaticQueueControlSetting(Material material)
        {
            // If a Shader Graph material doesn't yet have the queue control property,
            // we should not engage automatic behavior until the shader gets reimported.
            bool automaticQueueControl = !material.IsShaderGraph();
            if (material.HasProperty(Property.QueueControl))
            {
                var queueControl = material.GetFloat(Property.QueueControl);
                if (queueControl < 0.0f)
                {
                    // The property was added with a negative value, indicating it needs to be validated for this material
                    UpdateMaterialRenderQueueControl(material);
                }
                automaticQueueControl = (material.GetFloat(Property.QueueControl) == (float)QueueControl.Auto);
            }
            return automaticQueueControl;
        }
        private static void UpdateMaterialRenderQueueControl(Material material)
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
            bool isShaderGraph = material.IsShaderGraph(); // Non-shadergraph materials use automatic behavior
            
            // int rawRenderQueue = MaterialAccess.ReadMaterialRawRenderQueue(material);
            int rawRenderQueue = material.renderQueue; // TODO: 生のrenderQueueはinternalで取れそうにないのでとりあえず普通のやつ
            
            if (!isShaderGraph || rawRenderQueue == -1)
            {
                material.SetFloat(Property.QueueControl, (float)QueueControl.Auto); // Automatic behavior - surface type override
            }
            else
            {
                material.SetFloat(Property.QueueControl, (float)QueueControl.UserOverride); // User has selected explicit render queue
            }
        }
        private void OnOpenGUI(Material material, MaterialEditor materialEditor)
        {
            var filter = (Expandable)_materialFilter;

            // Generate the foldouts
            if (filter.HasFlag(Expandable.SurfaceOptions))
                _materialScopeList.RegisterHeaderScope(Styles.SurfaceOptions, (uint)Expandable.SurfaceOptions, DrawSurfaceOptions);

            if (filter.HasFlag(Expandable.SurfaceInputs))
                _materialScopeList.RegisterHeaderScope(Styles.SurfaceInputs, (uint)Expandable.SurfaceInputs, DrawSurfaceInputs);

            // NOTE: Unlitは未使用の模様。なんか活かせるかも
            // if (filter.HasFlag(Expandable.Details))
            //     FillAdditionalFoldouts(m_MaterialScopeList);

            if (filter.HasFlag(Expandable.Advanced))
                _materialScopeList.RegisterHeaderScope(Styles.AdvancedLabel, (uint)Expandable.Advanced, DrawAdvancedOptions);
        }

        private void DoPopup(GUIContent label, MaterialProperty property, string[] options)
        {
            if (property != null)
                _materialEditor.PopupShaderProperty(property, label, options);
            // NOTE: UTS3に同じ名前のメソッドがあるらしい。要注意
        }

        /// <summary>
        /// Draws the surface options GUI.
        /// </summary>
        /// <param name="material">The material to use.</param>
        public virtual void DrawSurfaceOptions(Material material)
        {
            DoPopup(Styles.surfaceType, _surfaceTypeProp, Styles.surfaceTypeNames);
            if ((_surfaceTypeProp != null) && ((SurfaceType)_surfaceTypeProp.floatValue == SurfaceType.Transparent))
            {
                DoPopup(Styles.blendingMode, _blendModeProp, Styles.blendModeNames);

                if (material.HasProperty(Property.BlendModePreserveSpecular))
                {
                    BlendMode blendMode = (BlendMode)material.GetFloat(Property.BlendMode);
                    var isDisabled = blendMode == BlendMode.Multiply || blendMode == BlendMode.Premultiply;
                    if (!isDisabled)
                        DrawFloatToggleProperty(Styles.preserveSpecularText, _preserveSpecProp, 1, isDisabled);
                }
            }
            DoPopup(Styles.cullingText, _cullingProp, Styles.renderFaceNames);
            DoPopup(Styles.zwriteText, _zwriteProp, Styles.zwriteNames);

            if (_ztestProp != null)
                _materialEditor.IntPopupShaderProperty(_ztestProp, Styles.ztestText.text, Styles.ztestNames, Styles.ztestValues);

            DrawFloatToggleProperty(Styles.alphaClipText, _alphaClipProp);

            if ((_alphaClipProp != null) && (_alphaCutoffProp != null) && (_alphaClipProp.floatValue == 1))
                _materialEditor.ShaderProperty(_alphaCutoffProp, Styles.alphaClipThresholdText, 1);

            DrawFloatToggleProperty(Styles.castShadowText, _castShadowsProp);
            DrawFloatToggleProperty(Styles.receiveShadowText, _receiveShadowsProp);
        }
        
        private static void DrawFloatToggleProperty(GUIContent styles, MaterialProperty prop, int indentLevel = 0, bool isDisabled = false)
        {
            if (prop == null)
                return;

            EditorGUI.BeginDisabledGroup(isDisabled);
            EditorGUI.indentLevel += indentLevel;
            EditorGUI.BeginChangeCheck();
            MaterialEditor.BeginProperty(prop);
            bool newValue = EditorGUILayout.Toggle(styles, prop.floatValue == 1);
            if (EditorGUI.EndChangeCheck())
                prop.floatValue = newValue ? 1.0f : 0.0f;
            MaterialEditor.EndProperty();
            EditorGUI.indentLevel -= indentLevel;
            EditorGUI.EndDisabledGroup();
        }
        
        private void ShaderPropertiesGUI(Material material)
        {
            _materialScopeList.DrawHeaders(_materialEditor, material);
        }
    }
}
