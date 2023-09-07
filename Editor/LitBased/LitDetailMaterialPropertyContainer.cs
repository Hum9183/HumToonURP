using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class LitDetailMaterialPropertyContainer : IMaterialPropertyContainer
    {
        private readonly MaterialPropertySetter _matPropSetter;

        public MaterialProperty DetailMask;
        public MaterialProperty DetailAlbedoMapScale;
        public MaterialProperty DetailAlbedoMap;
        public MaterialProperty DetailNormalMapScale;
        public MaterialProperty DetailNormalMap;

        public LitDetailMaterialPropertyContainer(MaterialPropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
