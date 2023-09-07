using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor.LitBased
{
    public static class RenderQueueSetter
    {
        public static void Set(Material material, bool isOpaque, bool alphaClip)
        {
            int renderQueue;

            if (isOpaque)
            {
                renderQueue = alphaClip ? (int)RenderQueue.AlphaTest : (int)RenderQueue.Geometry;
            }
            else
            {
                renderQueue = (int)RenderQueue.Transparent;
            }

            renderQueue += (int)material.GetFloat(HumToonPropertyNames.QueueOffset);

            if (material.renderQueue != renderQueue)
                material.renderQueue = renderQueue;
        }
    }
}
