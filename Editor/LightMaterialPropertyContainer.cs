using UnityEditor;

namespace HumToon.Editor
{
    public class LightMaterialPropertyContainer : IMaterialPropertyContainer
    {
        private readonly MaterialPropertySetter _matPropSetter;

        public MaterialProperty MainLightColorWeight;
        public MaterialProperty AdditionalLightColorWeight;

        public LightMaterialPropertyContainer(MaterialPropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
