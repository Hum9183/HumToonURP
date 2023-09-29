using Hum.HumToon.Editor.HeaderScopes;
using UnityEditor;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    public class LitPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _matPropSetter;
        // SmoothnessMap
        public MaterialProperty WorkflowMode;
        public MaterialProperty Metallic;
        public MaterialProperty SpecColor;
        public MaterialProperty MetallicGlossMap;
        public MaterialProperty SpecGlossMap;
        public MaterialProperty Smoothness;
        public MaterialProperty SmoothnessTextureChannel;
        public MaterialProperty BumpMap;
        public MaterialProperty BumpScale;
        public MaterialProperty ParallaxMap;
        public MaterialProperty Parallax;
        public MaterialProperty OcclusionStrength;
        public MaterialProperty OcclusionMap;
        public MaterialProperty SpecularHighlights;
        public MaterialProperty EnvironmentReflections;
        // public MaterialProperty ClearCoat;
        // public MaterialProperty ClearCoatMap;
        // public MaterialProperty ClearCoatMask;
        // public MaterialProperty ClearCoatSmoothness;

        public LitPropertiesContainer(PropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set(MaterialProperty[] materialProperties)
        {
            _matPropSetter.Set(this, materialProperties);
        }
    }
}
