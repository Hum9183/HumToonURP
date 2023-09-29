using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.Base
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

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
