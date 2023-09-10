using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class LitDetailPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _matPropSetter;

        public MaterialProperty DetailMask;
        public MaterialProperty DetailAlbedoMapScale;
        public MaterialProperty DetailAlbedoMap;
        public MaterialProperty DetailNormalMapScale;
        public MaterialProperty DetailNormalMap;

        public LitDetailPropertyContainer(PropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
