using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.MatCap
{
    public class MatCapPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty UseMatCap;
        public MaterialProperty MatCapMap;
        public MaterialProperty MatCapColor;
        public MaterialProperty MatCapIntensity;
        public MaterialProperty MatCapMapMipLevel;
        public MaterialProperty MatCapCorrectPerspectiveDistortion;
        public MaterialProperty MatCapStabilizeCameraZRotation;
        public MaterialProperty MatCapMainLightEffectiveness;

        public MatCapPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
