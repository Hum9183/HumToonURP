using UnityEditor;

namespace HumToon.Editor
{
    public class NormalPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty BumpMap;
        public MaterialProperty BumpScale;

        public NormalPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
