using UnityEngine;

namespace HumToon.Editor
{
    public static class AssignNewShaderToMaterialUtils
    {
        public static void ConvertEmission(Material material)
        {
            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }
        }
    }
}
