using Hum.HumToon.Editor.HeaderScopes;
using UnityEditor;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    public class LitPropertiesContainer : IPropertiesContainer
    {
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

        public void Set(MaterialProperty[] materialProperties)
        {
            PropertySetter.Set(this, materialProperties);
        }
    }
}
