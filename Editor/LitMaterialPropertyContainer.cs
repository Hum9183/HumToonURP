using UnityEditor;

namespace HumToon.Editor
{
    public class LitMaterialPropertyContainer : IMaterialPropertyContainer
    {
        private readonly MaterialPropertySetter _matPropSetter;

        public MaterialProperty WorkflowMode;
        public MaterialProperty Metallic;
        public MaterialProperty SpecColor;
        public MaterialProperty MetallicGlossMap;
        public MaterialProperty SpecGlossMap;
        public MaterialProperty Smoothness;
        public MaterialProperty SmoothnessMapChannel;
        public MaterialProperty BumpMap;
        public MaterialProperty BumpScale;
        public MaterialProperty ParallaxMap;
        public MaterialProperty ParallaxScale;
        public MaterialProperty OcclusionStrength;
        public MaterialProperty OcclusionMap;
        public MaterialProperty SpecularHighlights;
        public MaterialProperty EnvironmentReflections;
        public MaterialProperty ClearCoat;
        public MaterialProperty ClearCoatMap;
        public MaterialProperty ClearCoatMask;
        public MaterialProperty ClearCoatSmoothness;

        public LitMaterialPropertyContainer(MaterialPropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
