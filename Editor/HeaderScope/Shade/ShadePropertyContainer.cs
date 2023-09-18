using UnityEditor;

namespace HumToon.Editor
{
    public class ShadePropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty UseFirstShade;
        public MaterialProperty FirstShadeMap;
        public MaterialProperty FirstShadeColor;
        public MaterialProperty FirstShadeBorderPos;
        public MaterialProperty FirstShadeBorderBlur;

        public MaterialProperty UseExFirstShade;
        public MaterialProperty ExFirstShadeColor;
        public MaterialProperty ExFirstShadeWidth;

        public MaterialProperty UseSecondShade;
        public MaterialProperty SecondShadeMap;
        public MaterialProperty SecondShadeColor;
        public MaterialProperty SecondShadeBorderPos;
        public MaterialProperty SecondShadeBorderBlur;

        public ShadePropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
