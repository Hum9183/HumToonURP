using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes
{
    public static class PropertySetter
    {
        public static void Set<T>(T matPropContainer, MaterialProperty[] materialProperties)
            where T : IPropertiesContainer
        {
            var fieldInfos = typeof(T).GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                string matPropName = fieldInfo.Name.Prefix();
                var prop = FindProperty(matPropName, materialProperties, false);
                fieldInfo.SetValue(matPropContainer, prop);
            }
        }

        private static MaterialProperty FindProperty(string matPropName, MaterialProperty[] materialProperties, bool propertyIsMandatory)
        {
            foreach (var prop in materialProperties)
            {
                if (prop != null && prop.name.Equals(matPropName))
                    return prop;
            }

            if (propertyIsMandatory)
                throw new ArgumentException($"Could not find MaterialProperty: '{matPropName}', Num properties: {materialProperties.Length.ToString()}");

            return null;
        }
    }
}
