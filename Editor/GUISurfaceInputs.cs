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
    internal partial class HumToonGUI
    {
        private void DrawSurfaceInputs(Material material)
        {
            _materialEditor.TexturePropertySingleLine(Styles.baseMap, _matProps.BaseMap, _matProps.BaseColor);
            _materialEditor.TextureScaleOffsetProperty(_matProps.BaseMap);
        }
    }
}
