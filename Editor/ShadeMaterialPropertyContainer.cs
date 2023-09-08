using UnityEditor;

namespace HumToon.Editor
{
    public class ShadeMaterialPropertyContainer : IMaterialPropertyContainer
    {
        private readonly MaterialPropertySetter _matPropSetter;

        public MaterialProperty FirstShadeMap;
        public MaterialProperty FirstShadeColor;
        public MaterialProperty FirstShadeBorderPos;
        public MaterialProperty FirstShadeBorderBlur;

        public ShadeMaterialPropertyContainer(MaterialPropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
