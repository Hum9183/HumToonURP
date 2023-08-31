using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public partial class HumToonInspector
    {
        /// <summary>
        /// Called when a material has been changed.
        /// </summary>
        public override void ValidateMaterial(Material material)
        {
            Debug.Log("ValidateMaterial");

            int renderQueue = MaterialBlendModeSetter.Set(material);
            Utils.UpdateMaterialRenderQueue(material, renderQueue);

            MaterialKeywordsSetter.Set(material, litDetail: true);
        }
    }
}
