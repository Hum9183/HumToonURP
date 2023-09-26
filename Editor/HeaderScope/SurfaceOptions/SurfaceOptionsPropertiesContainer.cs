using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScope.SurfaceOptions
{
    public class SurfaceOptionsPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty SurfaceType;
        public MaterialProperty BlendMode;
        public MaterialProperty CullMode;
        public MaterialProperty AlphaClip;
        public MaterialProperty ReceiveShadows;
        public MaterialProperty Cutoff;

        public SurfaceOptionsPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
