using UnityEditor;

namespace HumToon.Editor
{
    public class BasePropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty BaseMap;
        public MaterialProperty BaseColor;

        public BasePropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
