using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScope.Base
{
    public class BasePropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty BaseMap;
        public MaterialProperty BaseColor;

        public BasePropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
