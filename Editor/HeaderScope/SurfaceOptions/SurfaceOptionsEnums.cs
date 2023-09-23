namespace HumToon.Editor
{
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
