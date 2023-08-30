using System;
using UnityEngine;
using UnityEditor;
using Unity.Rendering.Universal;
using ShaderPathID = UnityEngine.Rendering.Universal.ShaderPathID;
using UnityEditor.ShaderGraph;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEditor.Rendering.Universal.ShaderGUI;

namespace HumToon.Editor
{
    public static class Utils
    {
        public static bool ToBool(this float value)
        {
            return value >= 0.5;
        }

        public static float ToFloat(this bool value)
        {
            return value ? 1.0f : 0.0f;
        }
    }
}
