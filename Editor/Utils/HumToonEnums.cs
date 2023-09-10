using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace HumToon.Editor
{
    public class HumToonEnums
    {
    }

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
        SurfaceInputs = 1 << 1,

        /// <summary>
        /// Use this for advanced foldout.
        /// </summary>
        Advanced = 1 << 2,

        /// <summary>
        /// Use this for additional details foldout.
        /// </summary>
        Details = 1 << 3,

        Shade = 1 << 4,
        Light = 1 << 5,
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
    /// Options to select the texture channel where the smoothness value is stored.
    /// </summary>
    public enum SmoothnessSource
    {
        /// <summary>
        /// Use this when smoothness is stored in the alpha channel of the specular map.
        /// </summary>
        SpecularAlpha,

        /// <summary>
        /// Use this when smoothness is stored in the alpha channel of the base map.
        /// </summary>
        BaseAlpha,
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

    /// <summary>
    /// The options for controlling the render queue.
    /// </summary>
    public enum QueueControl
    {
        /// <summary>
        /// Use this to select automatic behavior.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Use this for explicitly selecting a render queue.
        /// </summary>
        UserOverride = 1
    }
    enum ZTestMode  // the values here match UnityEngine.Rendering.CompareFunction
    {
        Disabled = 0,
        Never = 1,
        Less = 2,
        Equal = 3,
        LEqual = 4,     // default for most rendering
        Greater = 5,
        NotEqual = 6,
        GEqual = 7,
        Always = 8,
    }

    enum ZWriteControl
    {
        Auto = 0,
        ForceEnabled = 1,
        ForceDisabled = 2
    }

    enum MaterialUpdateType
    {
        CreatedNewMaterial,
        ChangedAssignedShader,
        ModifiedShader,
        ModifiedMaterial
    }

    /// <summary>
    /// Workflow modes for the shader.
    /// </summary>
    public enum WorkflowMode
    {
        /// <summary>
        /// Use this for specular workflow.
        /// </summary>
        Specular = 0,

        /// <summary>
        /// Use this for metallic workflow.
        /// </summary>
        Metallic
    }

    /// <summary>
    /// Options to select the texture channel where the smoothness value is stored.
    /// </summary>
    public enum SmoothnessTextureChannel
    {
        /// <summary>
        /// Use this when smoothness is stored in the alpha channel of the Specular/Metallic Map.
        /// </summary>
        SpecularMetallicAlpha,

        /// <summary>
        /// Use this when smoothness is stored in the alpha channel of the Albedo Map.
        /// </summary>
        AlbedoAlpha,
    }
}
