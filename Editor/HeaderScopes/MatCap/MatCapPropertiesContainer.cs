using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public class MatCapPropertiesContainer : IPropertiesContainer
    {
        public MaterialProperty UseMatCap;
        public MaterialProperty MatCapMap;
        public MaterialProperty MatCapColor;
        public MaterialProperty MatCapIntensity;
        public MaterialProperty MatCapMapMipLevel;
        public MaterialProperty MatCapCorrectPerspectiveDistortion;
        public MaterialProperty MatCapStabilizeCameraZRotation;
        public MaterialProperty MatCapMask;
        public MaterialProperty MatCapMaskIntensity;
        public MaterialProperty MatCapMainLightEffectiveness;
    }
}
