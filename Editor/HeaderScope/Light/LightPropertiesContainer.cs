using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScope.Light
{
    public class LightPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty MainLightColorWeight;
        public MaterialProperty UseMainLightUpperLimit;
        public MaterialProperty MainLightUpperLimit;
        public MaterialProperty UseMainLightLowerLimit;
        public MaterialProperty MainLightLowerLimit;
        public MaterialProperty AdditionalLightsColorWeight;

        public LightPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
