using System;
using UnityEditor;

namespace HumToon.Editor
{
    public class PropertySetter
    {
        public MaterialProperty[] MatProps { private get; set; }

        public void Set<T>(T matPropContainer) where T : IPropertyContainer
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
            foreach (var prop in MatProps)
            {
                if (prop != null && prop.name.Equals(matPropName))
                    return prop;
            }

            if (propertyIsMandatory)
                throw new ArgumentException($"Could not find MaterialProperty: '{matPropName}', Num properties: {MatProps.Length.ToString()}");

            return null;
        }
    }
}
