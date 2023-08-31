using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            // NOTE: 自身がnewShaderのときに呼ばれる
            Debug.Log("AssignNewShaderToMaterial");

            if (material is null)
                throw new ArgumentNullException(nameof(material));

            ConvertEmission(material);

            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;

            AssignNewShader(material, oldShader, newShader);

            // Setup keywords based on the new shader
            MaterialKeywordsSetter.Set(material);

            if (oldShader is null || oldShader.name.Contains("Legacy Shaders/") is false)
            {
                int renderQueue = MaterialBlendModeSetter.Set(material);
                Utils.UpdateMaterialRenderQueue(material, renderQueue);
            }
            else
            {
                ModeLegacyShaders(material, oldShader);
            }
        }

        private void ConvertEmission(Material material)
        {
            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }
        }

        private void AssignNewShader(Material material, Shader oldShader, Shader newShader)
        {
            base.AssignNewShaderToMaterial(material, oldShader, newShader);
        }

        private void ModeLegacyShaders(Material material, Shader oldShader)
        {
            // mode: legacy shaders or null

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
    }
}
