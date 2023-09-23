using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace HumToon.Editor
{
    /// <summary>
    /// Flags for the foldouts used in the base shader GUI.
    /// </summary>
    [Flags]
    enum Expandable
    {
        /// <summary>
        /// Use this for surface options foldout.
        /// </summary>
        SurfaceOptions = 1 << 0,

        /// <summary>
        /// Use this for surface input foldout.
        /// </summary>
        Base = 1 << 1,

        Normal = 1 << 2,

        /// <summary>
        /// Use this for advanced foldout.
        /// </summary>
        Advanced = 1 << 3,

        /// <summary>
        /// Use this for additional details foldout.
        /// </summary>
        Details = 1 << 4,

        Shade = 1 << 5,
        RimLight = 1 << 6,
        Emission = 1 << 7,
        MatCap = 1 << 8,
        Light = 1 << 9,
    }
}
