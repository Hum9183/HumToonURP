using UnityEditor;

namespace HumToon.Editor
{
    public class LightPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty MainLightColorWeight;
        public MaterialProperty AdditionalLightColorWeight;

        public LightPropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
