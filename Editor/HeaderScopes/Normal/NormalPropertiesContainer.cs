using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.Normal
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

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
