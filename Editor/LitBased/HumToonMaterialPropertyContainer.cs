using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class HumToonMaterialPropertyContainer : IMaterialPropertyContainer
    {
        private readonly MaterialPropertySetter _matPropSetter;

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

        public HumToonMaterialPropertyContainer(MaterialPropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}
