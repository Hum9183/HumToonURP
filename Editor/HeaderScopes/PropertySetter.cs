using System;
using Hum.HumToon.Editor.Utils;
using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes
{
    public class PropertySetter
    {
        public MaterialProperty[] MatProps { set => _matProps = value ?? throw new ArgumentException(); }
        private MaterialProperty[] _matProps;

        public void Set<T>(T matPropContainer) where T : IPropertiesContainer
        {
            var fieldInfos = typeof(T).GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                string matPropName = fieldInfo.Name.Prefix();
                var prop = FindProperty(matPropName, false);
                fieldInfo.SetValue(matPropContainer, prop);
            }
        }

        private MaterialProperty FindProperty(string matPropName, bool propertyIsMandatory)
        {
            foreach (var prop in _matProps)
            {
                if (prop != null && prop.name.Equals(matPropName))
                    return prop;
            }

            if (propertyIsMandatory)
                throw new ArgumentException($"Could not find MaterialProperty: '{matPropName}', Num properties: {_matProps.Length.ToString()}");

            return null;
        }
    }
}
