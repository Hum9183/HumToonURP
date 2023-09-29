using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.RimLight
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

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
