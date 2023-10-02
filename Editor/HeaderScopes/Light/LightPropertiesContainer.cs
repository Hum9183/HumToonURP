using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.Light
{
    public class LightPropertiesContainer : IPropertiesContainer
    {
        public MaterialProperty MainLightColorWeight;
        public MaterialProperty UseMainLightUpperLimit;
        public MaterialProperty MainLightUpperLimit;
        public MaterialProperty UseMainLightLowerLimit;
        public MaterialProperty MainLightLowerLimit;
        public MaterialProperty AdditionalLightsColorWeight;
    }
}
