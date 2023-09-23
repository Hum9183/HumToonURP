using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class LitDetailPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _matPropSetter;

        public MaterialProperty DetailMask;
        public MaterialProperty DetailAlbedoMapScale;
        public MaterialProperty DetailAlbedoMap;
        public MaterialProperty DetailNormalMapScale;
        public MaterialProperty DetailNormalMap;

        public LitDetailPropertiesContainer(PropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
