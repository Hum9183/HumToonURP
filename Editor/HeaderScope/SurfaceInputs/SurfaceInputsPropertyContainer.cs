using UnityEditor;

namespace HumToon.Editor
{
    public class SurfaceInputsPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty BaseMap;
        public MaterialProperty BaseColor;

        public SurfaceInputsPropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
