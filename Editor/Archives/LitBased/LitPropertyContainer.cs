using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class LitPropertyContainer : IPropertyContainer
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

        public LitPropertyContainer(PropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
