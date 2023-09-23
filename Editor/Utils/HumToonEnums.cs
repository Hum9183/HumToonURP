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

    /// <summary>
    /// The surface type for your object.
    /// </summary>
    public enum SurfaceType
    {
        /// <summary>
        /// Use this for opaque surfaces.
        /// </summary>
        Opaque,

        /// <summary>
        /// Use this for transparent surfaces.
        /// </summary>
        Transparent
    }

    /// <summary>
    /// The blend mode for your material.
    /// </summary>
    public enum TransparentBlendMode
    {
        /// <summary>
        /// Use this for alpha blend mode.
        /// </summary>
        Alpha,   // Old school alpha-blending mode, fresnel does not affect amount of transparency

        /// <summary>
        /// Use this for premultiply blend mode.
        /// </summary>
        Premultiply, // Physically plausible transparency mode, implemented as alpha pre-multiply

        /// <summary>
        /// Use this for additive blend mode.
        /// </summary>
        Additive,

        /// <summary>
        /// Use this for multiply blend mode.
        /// </summary>
        Multiply
    }

    /// <summary>
    /// The face options to render your geometry.
    /// </summary>
    public enum RenderFace
    {
        /// <summary>
        /// Use this to render only front face.
        /// </summary>
        Front = 2,

        /// <summary>
        /// Use this to render only back face.
        /// </summary>
        Back = 1,

        /// <summary>
        /// Use this to render both faces.
        /// </summary>
        Both = 0
    }
}
