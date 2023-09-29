using Hum.HumToon.Editor.HeaderScopes;
using UnityEditor;

namespace Hum.HumToon.Editor.Archives.LitBased
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

        public void Set(MaterialProperty[] materialProperties)
        {
            _matPropSetter.Set(this, materialProperties);
        }
    }
}
