using Hum.HumToon.Editor.HeaderScopes;
using UnityEditor;

namespace Hum.HumToon.Editor.Archives.LitBased
{
    public class HumToonPropertiesContainer : IPropertiesContainer
    {
        public MaterialProperty SurfaceType;
        public MaterialProperty BlendMode;
        public MaterialProperty BlendModePreserveSpecular;
        public MaterialProperty CullMode;
        // public MaterialProperty ZWriteControl;
        public MaterialProperty AlphaClip;
        public MaterialProperty ReceiveShadows;
        public MaterialProperty Cutoff;
        public MaterialProperty BaseMap;
        public MaterialProperty BaseColor;
        public MaterialProperty EmissionMap;
        public MaterialProperty EmissionColor;
        public MaterialProperty QueueOffset;

        public void Set(MaterialProperty[] materialProperties)
        {
            PropertySetter.Set(this, materialProperties);
        }
    }
}
