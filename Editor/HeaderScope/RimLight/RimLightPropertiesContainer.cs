using UnityEditor;

namespace HumToon.Editor
{
    public class RimLightPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty UseRimLight;
        public MaterialProperty RimLightMap;
        public MaterialProperty RimLightColor;
        public MaterialProperty RimLightIntensity;
        public MaterialProperty RimLightBorderPos;
        public MaterialProperty RimLightBorderBlur;
        public MaterialProperty RimLightMainLightEffectiveness;

        public RimLightPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
