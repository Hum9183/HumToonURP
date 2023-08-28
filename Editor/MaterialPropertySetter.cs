using System;
using UnityEditor;

namespace HumToon.Editor
{
    public class MaterialPropertySetter
    {
        public MaterialProperty[] MatProps { private get; set; }

        public void Set<T>(T matPropContainer) where T : IMaterialPropertyContainer
        {
            var fieldInfos = typeof(T).GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                string matPropName = $"_{fieldInfo.Name}";
                var prop = FindProperty(matPropName, false);
                fieldInfo.SetValue(matPropContainer, prop);
            }
        }

        private MaterialProperty FindProperty(string matPropName, bool propertyIsMandatory)
        {
            // NOTE: Linqで書いてもいいかも
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
