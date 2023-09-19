using UnityEditor;

namespace HumToon.Editor
{
    public class NormalPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty BumpMap;
        public MaterialProperty BumpScale;

        public NormalPropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
