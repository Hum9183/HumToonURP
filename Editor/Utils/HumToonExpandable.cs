using System;

namespace Hum.HumToon.Editor.Utils
{
    /// <summary>
    /// Flags for the foldouts used in the Hum Toon GUI.
    /// </summary>
    [Flags]
    public enum Expandable
    {
        SurfaceOptions = 1 << 0,
        Base = 1 << 1,
        Normal = 1 << 2,
        Shade = 1 << 3,
        RimLight = 1 << 4,
        Emission = 1 << 5,
        MatCap = 1 << 6,
        Light = 1 << 7,
    }
}
