using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes
{
    public interface IHeaderScopeDrawer
    {
        void SetProperties(MaterialProperty[] materialProperties);
        void Draw(MaterialEditor materialEditor);
    }
}
