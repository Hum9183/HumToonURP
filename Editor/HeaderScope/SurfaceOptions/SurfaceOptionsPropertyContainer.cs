using UnityEditor;

namespace HumToon.Editor
{
    public class SurfaceOptionsPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty SurfaceType;
        public MaterialProperty BlendMode;
        public MaterialProperty CullMode;
        public MaterialProperty AlphaClip;
        public MaterialProperty ReceiveShadows;
        public MaterialProperty Cutoff;

        public SurfaceOptionsPropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
