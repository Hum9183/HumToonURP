using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.SurfaceOptions
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

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
